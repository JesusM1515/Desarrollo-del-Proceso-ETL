using Application.DTO;
using Application.Interfaces;
using Application.Interfaces.IRepositories;
using Application.Mapping.CSV;
using Domain.Entities.DWH.Dimensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.Services.DWH
{
    public class DWHHandlerServices : IDWHHandlerServices
    {
        private readonly IFileReaderRepository<CSVClienteDTO> _clienteReader;
        private readonly IFileReaderRepository<CSVProductoDTO> _productoReader;
        private readonly IFileReaderRepository<CSVFuentesDTO> _fuentesReader;

        // Aquí está la clave: Inyectamos la INTERFAZ del repositorio, no el contexto.
        private readonly IDWHRepository _dwhRepository;
        private readonly IPathProvider _pathProvider;
        private readonly ILogger<DWHHandlerServices> _logger;

        public DWHHandlerServices(
            IFileReaderRepository<CSVClienteDTO> clienteReader,
            IFileReaderRepository<CSVProductoDTO> productoReader,
            IFileReaderRepository<CSVFuentesDTO> fuentesReader,
            IDWHRepository dwhRepository, // <--- Solo pedimos el repositorio
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

        public async Task LoadDataWarehouseAsync()
        {
            _logger.LogInformation("Obteniendo rutas de archivos");
            var pathClientes = _pathProvider.GetCsvClientes();
            var pathProductos = _pathProvider.GetCsvProductos();
            var pathFuentes = _pathProvider.GetCsvFuentes();

            _logger.LogInformation("Leyendo CSVs");
            var dtosClientes = await _clienteReader.ReadFileAsync(pathClientes, new CSVClienteMap());
            var dtosProductos = await _productoReader.ReadFileAsync(pathProductos, new CSVProductoMap());
            var dtosFuentes = await _fuentesReader.ReadFileAsync(pathFuentes, new CSVFuenteMap());

            _logger.LogInformation("3. Transformando datos a Entidades de DWH...");
            var entidadesParaGuardar = new DimSourceDataDTO();

            //Mapeo
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

            entidadesParaGuardar.Productos = dtosProductos.Select(p => new Dim_Producto
            {
                ID_Producto = p.ID_Producto,
                Nombre = p.Nombre,
                Marca = p.Marca,
                Precio = p.Precio,
                FK_Categoria = p.FK_Categoria
            }).ToList();

            entidadesParaGuardar.Fuentes = dtosFuentes.Select(f => new Dim_FuentesDatos
            {
                ID_FuenteDatos = f.ID_FuenteDatos,
                NombreFuenteDatos = f.NombreFuenteDatos,
                TipoFuenteDatos = f.TipoFuenteDatos,
                Plataforma = f.Plataforma
            }).ToList();

            // Generar dimensiones estáticas
            entidadesParaGuardar.Tiempo = GenerarTiempo(new DateTime(2023, 1, 1), new DateTime(2025, 12, 31));
            entidadesParaGuardar.Sentimientos = new List<Dim_Sentimiento>
        {
            new Dim_Sentimiento { ID_Sentimiento = 1, Clasificacion = "Positivo" },
            new Dim_Sentimiento { ID_Sentimiento = 2, Clasificacion = "Negativo" }
        };

            _logger.LogInformation("Enviando  al Repositorio");

            // AQUÍ es donde delegamos. El servicio dice: "Toma estos datos y guárdalos, no me importa cómo".
            await _dwhRepository.SaveProcessedDimensionsAsync(entidadesParaGuardar); //aqui esta el error
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
                    Anio = dia.Year,
                    Mes = dia.Month.ToString(),
                    Dia = dia.Day.ToString(),
                    Trimestres = (dia.Month - 1) / 3 + 1
                });
            }
            return lista;
        }
    }
}