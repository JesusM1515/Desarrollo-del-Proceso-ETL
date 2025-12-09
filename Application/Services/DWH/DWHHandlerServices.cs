using Application.DTO;
using Application.Interfaces;
using Application.Interfaces.IRepositories;
using Application.Mapping.CSV;
using Domain.Entities.Base;
using Domain.Entities.DWH.Dimensions;
using Microsoft.Extensions.Logging;

namespace Application.Services.DWH
{
    public class DWHHandlerServices : IDWHHandlerServices
    {
        private readonly IFileReaderRepository<CSVClienteDTO> _clienteReader;
        private readonly IFileReaderRepository<CSVProductoDTO> _productoReader;
        private readonly IFileReaderRepository<CSVFuentesDTO> _fuentesReader;
        private readonly IFileReaderRepository<CSVSurveyDTO> _SurveyReader;
        private readonly IDWHRepository _dwhRepository;
        private readonly IPathProvider _pathProvider;
        private readonly ILogger<DWHHandlerServices> _logger;

        public DWHHandlerServices(
            IFileReaderRepository<CSVClienteDTO> clienteReader,
            IFileReaderRepository<CSVProductoDTO> productoReader,
            IFileReaderRepository<CSVFuentesDTO> fuentesReader,
            IFileReaderRepository<CSVSurveyDTO> surveyReader,
            IDWHRepository dwhRepository,
            IPathProvider pathProvider,
            ILogger<DWHHandlerServices> logger)
        {
            _clienteReader = clienteReader;
            _productoReader = productoReader;
            _fuentesReader = fuentesReader;
            _SurveyReader = surveyReader;
            _dwhRepository = dwhRepository;
            _pathProvider = pathProvider;
            _logger = logger;
        }

        public async Task<OperationResult<DimSourceDataDTO>> LoadDataWarehouseAsync()
        {
            //Lectura de CSVs
            _logger.LogInformation("Obteniendo rutas de archivos");
            var pathClientes = _pathProvider.GetCsvClientes();
            var pathProductos = _pathProvider.GetCsvProductos();
            var pathFuentes = _pathProvider.GetCsvFuentes();
            var pathSurvey = _pathProvider.GetCsvSurveys();

            _logger.LogInformation("Leyendo CSVs");
            var dtosClientes = await _clienteReader.ReadFileAsync(pathClientes, new CSVClienteMap());
            var dtosProductos = await _productoReader.ReadFileAsync(pathProductos, new CSVProductoMap());
            var dtosFuentes = await _fuentesReader.ReadFileAsync(pathFuentes, new CSVFuenteMap());
            var dtoSurvey = await _SurveyReader.ReadFileAsync(pathSurvey, new CSVSurveyMap());

            _logger.LogInformation("Transformando datos a entidades del DWH");

            var entidadesParaGuardar = new DimSourceDataDTO();

            //Fact Opiniones
            entidadesParaGuardar.Opiniones = dtoSurvey.Select(op => new FactOpinionesDTO
            {
                ID_Opiniones = op.ID_Opiniones,
                Comentario = op.Comentario,
                ClasificacionRaw = op.ClasificacionRaw,
                PalabrasClave = string.Join(",", ExtraerPalabrasClave(op.Comentario)),
                PuntajeSatisfaccion = op.PuntajeSatisfaccion,
                FechaCarga = op.FechaCarga,
                FK_Clientes = op.FK_Clientes,
                FK_Producto = op.FK_Producto,
                ID_FuenteDatos = op.Fuente?.Trim()
            }).ToList();

            //Clientes
            var clientesDesdeArchivo = dtosClientes.Select(c => new Dim_Clientes
            {
                ID_Clientes = c.ID_Clientes,
                Nombre = c.Nombre,
                Edad = c.Edad,
                Email = c.Email,
                Pais = c.Pais,
                Ciudad = c.Ciudad,
                Tipo = c.Tipo
            }).ToList();

            var clientesDesdeOpiniones = entidadesParaGuardar.Opiniones
                .Select(o => o.FK_Clientes)
                .Distinct()
                .Select(id => new Dim_Clientes
                {
                    ID_Clientes = id,
                    Nombre = "Cliente Desconocido",
                    Email = "N/A",
                    Pais = "N/A",
                    Ciudad = "N/A",
                    Tipo = "N/A",
                    Edad = 0
                })
                .ToList();

            entidadesParaGuardar.Clientes = clientesDesdeArchivo
                .Concat(clientesDesdeOpiniones)
                .GroupBy(c => c.ID_Clientes)
                .Select(g => g.First())
                .ToList();

            //Productos
            var productosDesdeArchivo = dtosProductos.Select(p => new Dim_Producto
            {
                ID_Producto = p.ID_Producto,
                Nombre = p.Nombre,
                Marca = p.Marca,
                Precio = p.Precio,
                Categoria = p.Categoria
            }).ToList();

            var productosDesdeOpiniones = entidadesParaGuardar.Opiniones
                .Select(o => o.FK_Producto)
                .Distinct()
                .Select(id => new Dim_Producto
                {
                    ID_Producto = id,
                    Nombre = $"Producto {id} (Desconocido)",
                    Marca = "N/A",
                    Precio = 0,
                    Categoria = "Desconocido"
                })
                .ToList();

            entidadesParaGuardar.Productos = productosDesdeArchivo
                .Concat(productosDesdeOpiniones)
                .GroupBy(p => p.ID_Producto)
                .Select(g => g.First())
                .ToList();

            //Fuentes
            var fuentesDesdeArchivos = dtosFuentes.Select(f => new Dim_FuentesDatos
            {
                ID_FuenteDatos = f.ID_FuenteDatos?.Trim(),
                NombreFuenteDatos = f.NombreFuenteDatos,
                TipoFuenteDatos = f.TipoFuenteDatos,
                Plataforma = f.Plataforma,
                FechaCarga = f.FechaCarga
            }).ToList();

            var fuentesDesdeOpiniones = entidadesParaGuardar.Opiniones
                .Where(o => !string.IsNullOrEmpty(o.ID_FuenteDatos))
                .GroupBy(o => o.ID_FuenteDatos.Trim())
                .Select(g => new Dim_FuentesDatos
                {
                    ID_FuenteDatos = g.Key,
                    TipoFuenteDatos = "Survey",
                    NombreFuenteDatos = g.Key,
                    Plataforma = "Interna",
                    FechaCarga = DateTime.Today
                })
                .ToList();

            entidadesParaGuardar.Fuentes = fuentesDesdeArchivos
                .Concat(fuentesDesdeOpiniones)
                .GroupBy(f => f.ID_FuenteDatos?.Trim().ToLower())
                .Select(g => g.First())
                .ToList();

            //Fuente
            var listaTiempo = GenerarTiempo(new DateTime(2023, 1, 1), new DateTime(2025, 12, 31));
            entidadesParaGuardar.Tiempo = listaTiempo
                .GroupBy(t => t.ID_Tiempo)
                .Select(g => g.First())
                .ToList();

            entidadesParaGuardar.Sentimientos = entidadesParaGuardar.Opiniones
                .Where(o => !string.IsNullOrEmpty(o.ClasificacionRaw))
                .Select(o => o.ClasificacionRaw.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Select((clasificacion, index) => new Dim_Sentimiento
                {
                    ID_Sentimiento = index + 1,
                    Clasificacion = clasificacion
                })
                .GroupBy(s => s.Clasificacion.ToLower())
                .Select(g => g.First())
                .ToList();


            if (entidadesParaGuardar == null)
            {
                _logger.LogWarning("Los datos se detectaron como null");
                return OperationResult<DimSourceDataDTO>.Failure("Falla al cargar datos");
            }

            _logger.LogInformation("Enviando al Repositorio");
            await _dwhRepository.SaveProcessedDimensionsAsync(entidadesParaGuardar);

            return OperationResult<DimSourceDataDTO>.Success("Datos guardados con exito", entidadesParaGuardar);
        }

        //Tiempo estatico
        private List<Dim_Tiempo> GenerarTiempo(DateTime inicio, DateTime fin)
        {
            var lista = new List<Dim_Tiempo>();
            for (var dia = inicio; dia <= fin; dia = dia.AddDays(1))
            {
                lista.Add(new Dim_Tiempo
                {
                    ID_Tiempo = int.Parse(dia.ToString("yyyyMMdd")),
                    Fecha = dia,
                    Year = dia.Year,
                    Mes = dia.Month.ToString(),
                    Dia = dia.Day.ToString(),
                    Trimestres = (dia.Month - 1) / 3 + 1
                });
            }
            return lista;
        }

        private List<string> ExtraerPalabrasClave(string comentario)
        {
            if (string.IsNullOrWhiteSpace(comentario)) return new List<string>();
            var stopwords = new HashSet<string> { "el", "la", "los", "las", "de", "y", "en", "un", "una", "con", "por", "para", "muy" };
            return comentario.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.Trim('.', ',', ';', ':', '!', '?'))
                .Where(p => !stopwords.Contains(p) && p.Length > 2).Distinct().ToList();
        }
    }
}