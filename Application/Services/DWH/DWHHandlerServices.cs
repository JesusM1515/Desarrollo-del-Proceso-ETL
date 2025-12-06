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
        private readonly IDWHRepository _dwhRepository;
        private readonly IPathProvider _pathProvider;
        private readonly ILogger<DWHHandlerServices> _logger;

        public DWHHandlerServices(
            IFileReaderRepository<CSVClienteDTO> clienteReader,
            IFileReaderRepository<CSVProductoDTO> productoReader,
            IFileReaderRepository<CSVFuentesDTO> fuentesReader,
            IDWHRepository dwhRepository,
            IPathProvider pathProvider,
            ILogger<DWHHandlerServices> logger)
        {
            _clienteReader = clienteReader;
            _productoReader = productoReader;
            _fuentesReader = fuentesReader;
            _dwhRepository = dwhRepository;
            _pathProvider = pathProvider;
            _logger = logger;
        }

        //public async Task LoadDataWarehouseAsync()
        public async Task<OperationResult<DimSourceDataDTO>> LoadDataWarehouseAsync()
        {
            _logger.LogInformation("Obteniendo rutas de archivos");

            var pathClientes = _pathProvider.GetCsvClientes();
            var pathProductos = _pathProvider.GetCsvProductos();
            var pathFuentes = _pathProvider.GetCsvFuentes();

            _logger.LogInformation("Leyendo CSVs");

            var dtosClientes = await _clienteReader.ReadFileAsync(pathClientes, new CSVClienteMap());
            var dtosProductos = await _productoReader.ReadFileAsync(pathProductos, new CSVProductoMap());
            var dtosFuentes = await _fuentesReader.ReadFileAsync(pathFuentes, new CSVFuenteMap());

            _logger.LogInformation("Transformando datos a entidades del DWH");

            var entidadesParaGuardar = new DimSourceDataDTO();

            //Mapeo
            //Actualizado
            entidadesParaGuardar.Clientes = dtosClientes.Select(c => new Dim_Clientes
            {
                ID_Clientes = c.ID_Clientes,
                Nombre = c.Nombre,
                Edad = c.Edad,
                Email = c.Email,
                Pais = c.Pais,
                Ciudad = c.Ciudad,
                Tipo = c.Tipo
            }).ToList();

            //Actualizado
            entidadesParaGuardar.Productos = dtosProductos.Select(p => new Dim_Producto
            {
                ID_Producto = p.ID_Producto,
                Nombre = p.Nombre,
                Marca = p.Marca,
                Precio = p.Precio,
                Categoria = p.Categoria
            }).ToList();

            /*Actualizado (Potencialmente eliminado)
            entidadesParaGuardar.Productos = dtosProductos.Select(p => new Dim_Categoria
            {
                ID_Categoria = p.ID_Producto,
                Nombre = p.Nombre,
                Descripcion = "Descripcion pendiente"
            }).ToList(); */

            //Actualizado
            entidadesParaGuardar.Fuentes = dtosFuentes.Select(f => new Dim_FuentesDatos
            {
                ID_FuenteDatos = f.ID_FuenteDatos,
                NombreFuenteDatos = f.NombreFuenteDatos,
                TipoFuenteDatos = f.TipoFuenteDatos,
                Plataforma = f.Plataforma,
                FechaCarga = f.FechaCarga
            }).ToList();

            //Generar dimensiones estaticas
            entidadesParaGuardar.Tiempo = GenerarTiempo(new DateTime(2023, 1, 1), new DateTime(2025, 12, 31));
            entidadesParaGuardar.Sentimientos = new List<Dim_Sentimiento>
        {    //Actualizado
            new Dim_Sentimiento { ID_Sentimiento = 1, Clasificacion = "Positivo" },
            new Dim_Sentimiento { ID_Sentimiento = 2, Clasificacion = "Negativo" },
            new Dim_Sentimiento { ID_Sentimiento = 3, Clasificacion = "Neutra" }
        };

            if (entidadesParaGuardar == null)
            {
                _logger.LogWarning("Los datos se detectaron como null al procesar la informacion");
                OperationResult<DimSourceDataDTO>.Failure("Hubo una falla al cargar los datos a DimSourceDataDTO");
            }

            _logger.LogInformation("Enviando  al Repositorio");

            //Mandar a la base de datos para guardar
            await _dwhRepository.SaveProcessedDimensionsAsync(entidadesParaGuardar);

            return OperationResult<DimSourceDataDTO>.Success("Datos gaurdados en el DWH con exito", entidadesParaGuardar);

        }

        // Metodo para logica de fechas
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
    }
}