using Application.DTO;
using Application.Interfaces;
using Application.Interfaces.IRepositories;
using Domain.Entities.DWH.Dimensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Application.Services.DWH
{
    // Nombre de la clase: DWHHandlerServices
    public class DWHHandlerServices : IDWHHandlerServices // Asumiendo que IDWHHandlerServices es la interfaz correcta
    {
        private readonly IDWHRepository _dwhRepository;
        private readonly DWHContext _context;

        private readonly ILogger<DWHHandlerServices> _Logger;

        public DWHHandlerServices(IDWHRepository dwhRepository, DWHContext context, ILogger<DWHHandlerServices> logger)
        {
            _dwhRepository = dwhRepository;
            _context = context;
            _Logger = logger;
        }

        public async Task LoadDataWarehouseAsync(DimSourceDataDTO sourceData)
        {
            _Logger.LogInformation("Iniciando procesamiento ETL de Dimensiones");

            // Mapas en memoria de los existentes
            var clientesExistentes = await _context.Dim_Clientes.AsNoTracking().ToDictionaryAsync(c => c.ID_Clientes, c => c.Key_Clientes);
            var productosExistentes = await _context.Dim_Producto.AsNoTracking().ToDictionaryAsync(p => p.ID_Producto, p => p.Key_Producto);
            var categoriasExistentes = await _context.Dim_Categoria.AsNoTracking().ToDictionaryAsync(c => c.ID_Categoria, c => c.Key_Categoria);
            var fuentesExistentes = await _context.Dim_FuentesDatos.AsNoTracking().ToDictionaryAsync(f => f.ID_FuenteDatos, f => f.Key_FuenteDatos);

            // Listas para las entidades a insertar
            var nuevosClientes = new List<Dim_Clientes>();
            var nuevosProductos = new List<Dim_Producto>();
            var nuevasFuentes = new List<Dim_FuentesDatos>();
            var nuevasCategorias = new List<Dim_Categoria>();

            //Categorías
            foreach (var dto in sourceData.CategoriaDTO)
            {
                if (!categoriasExistentes.ContainsKey(dto.ID_Categoria))
                {
                    nuevasCategorias.Add(new Dim_Categoria
                    {
                        ID_Categoria = dto.ID_Categoria,
                        Nombre = LimpiarTexto(dto.Nombre),
                        Descripcion = LimpiarTexto(dto.Descripcion)
                    });
                }
            }

            //Fuentes de Datos
            foreach (var dto in sourceData.FuentesDTO)
            {
                if (!fuentesExistentes.ContainsKey(dto.ID_FuenteDatos))
                {
                    string nombreFuente = !string.IsNullOrWhiteSpace(dto.NombreFuenteDatos) ? LimpiarTexto(dto.NombreFuenteDatos) : $"Fuente {dto.ID_FuenteDatos}";

                    nuevasFuentes.Add(new Dim_FuentesDatos
                    {
                        ID_FuenteDatos = dto.ID_FuenteDatos,
                        TipoFuenteDatos = dto.TipoFuenteDatos,
                        NombreFuenteDatos = nombreFuente,
                        Plataforma = dto.Plataforma, // Usa valor por defecto del DTO
                    });
                }
            }

            //Clientes
            var clientesUnicos = sourceData.ClienteDTOs
                .Where(c => !string.IsNullOrWhiteSpace(c.Email))
                .GroupBy(c => c.Email.ToLower().Trim())
                .Select(g => g.First());

            foreach (var dto in clientesUnicos)
            {
                if (!clientesExistentes.ContainsKey(dto.ID_Clientes))
                {
                    string nombreNorm = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(LimpiarTexto(dto.Nombre).ToLower());

                    nuevosClientes.Add(new Dim_Clientes
                    {
                        ID_Clientes = dto.ID_Clientes,
                        Nombre = nombreNorm,
                        Email = dto.Email.ToLower().Trim(), // Normalización Email
                        Edad = dto.Edad, // Usa valor por defecto del DTO (0) si no vino en CSV
                        Pais = LimpiarTexto(dto.Pais),
                        Ciudad = LimpiarTexto(dto.Ciudad),
                        Tipo = dto.Tipo
                    });
                }
            }

            //Productos
            foreach (var dto in sourceData.ProductoDTO)
            {
                bool categoriaExiste = categoriasExistentes.ContainsKey(dto.FK_Categoria) || nuevasCategorias.Any(c => c.ID_Categoria == dto.FK_Categoria);

                if (!productosExistentes.ContainsKey(dto.ID_Producto) && categoriaExiste)
                {
                    nuevosProductos.Add(new Dim_Producto
                    {
                        ID_Producto = dto.ID_Producto,
                        Nombre = LimpiarTexto(dto.Nombre),
                        FK_Categoria = dto.FK_Categoria,
                        Marca = dto.Marca,
                        Precio = dto.Precio,
                    });
                }
            }

            //Escribir dimensiones (usando el Repositorio)
            await _dwhRepository.SaveProcessedDimensionsAsync(
                nuevasCategorias,
                nuevasFuentes,
                nuevosClientes,
                nuevosProductos);

            _Logger.LogInformation("Carga de dimensiones finalizada correctamente");
        }

        private string LimpiarTexto(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;
            string limpio = Regex.Replace(input, @"[^\w\s\.\-@áéíóúÁÉÍÓÚñÑ]", "");
            return limpio.Trim();
        }
    }
}