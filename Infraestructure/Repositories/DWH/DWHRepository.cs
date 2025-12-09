using Application.DTO;
using Application.Interfaces.IRepositories;
using Domain.Entities.DWH.Facts;
using Infraestructure.BD.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infraestructure.Repositories.DWH
{
    public class DWHRepository : IDWHRepository
    {
        private readonly DWHContext _context;
        private readonly ILogger<DWHRepository> _Logger;

        public DWHRepository(DWHContext context, ILogger<DWHRepository> logger)
        {
            _context = context;
            _Logger = logger;
        }

        public async Task SaveProcessedDimensionsAsync(DimSourceDataDTO data)
        {
            _Logger.LogInformation("Iniciando guardado de dimensiones y facts...");

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                //Fuente
                var fuentesExistentes = await _context.Dim_FuentesDatos
                    .Select(f => f.ID_FuenteDatos.Trim().ToLower())
                    .ToListAsync();

                if (data.Fuentes != null && data.Fuentes.Any())
                {
                    var nuevasFuentes = data.Fuentes
                        .GroupBy(f => f.ID_FuenteDatos.Trim().ToLower())
                        .Select(g => g.First())
                        .Where(nueva => !fuentesExistentes.Contains(nueva.ID_FuenteDatos.Trim().ToLower()))
                        .Select(f => new Domain.Entities.DWH.Dimensions.Dim_FuentesDatos
                        {
                            ID_FuenteDatos = f.ID_FuenteDatos.Trim().ToLower(),
                            TipoFuenteDatos = f.TipoFuenteDatos,
                            NombreFuenteDatos = f.NombreFuenteDatos,
                            Plataforma = f.Plataforma,
                            FechaCarga = f.FechaCarga
                        })
                        .ToList();

                    if (nuevasFuentes.Any())
                    {
                        await _context.Dim_FuentesDatos.AddRangeAsync(nuevasFuentes);
                    }
                }

                //Clientes
                if (data.Clientes != null && data.Clientes.Any())
                {
                    var clientesExistentes = await _context.Dim_Clientes.Select(c => c.ID_Clientes).ToListAsync();
                    var nuevosClientes = data.Clientes.Where(c => !clientesExistentes.Contains(c.ID_Clientes)).ToList();

                    if (nuevosClientes.Any())
                        await _context.Dim_Clientes.AddRangeAsync(nuevosClientes);
                }

                //Productos
                if (data.Productos != null && data.Productos.Any())
                {
                    var productosExistentes = await _context.Dim_Producto.Select(p => p.ID_Producto).ToListAsync();
                    var nuevosProductos = data.Productos.Where(p => !productosExistentes.Contains(p.ID_Producto)).ToList();

                    if (nuevosProductos.Any())
                        await _context.Dim_Producto.AddRangeAsync(nuevosProductos);
                }

                //Tiempo
                if (data.Tiempo != null && data.Tiempo.Any())
                {
                    var tiempoExistente = await _context.Dim_Tiempo.Select(t => t.ID_Tiempo).ToListAsync();
                    var nuevoTiempo = data.Tiempo.GroupBy(t => t.ID_Tiempo).Select(g => g.First()).Where(t => !tiempoExistente.Contains(t.ID_Tiempo)).ToList();
                    if (nuevoTiempo.Any()) await _context.Dim_Tiempo.AddRangeAsync(nuevoTiempo);
                }

                //Sentimiento
                var clasificacionesExistentes = await _context.Dim_Sentimiento.Select(s => s.Clasificacion.Trim().ToLower()).ToListAsync();
                if (data.Sentimientos != null && data.Sentimientos.Any())
                {
                    var nuevosSentimientos = data.Sentimientos
                        .GroupBy(s => s.Clasificacion.Trim().ToLower())
                        .Select(g => g.First())
                        .Where(nuevo => !clasificacionesExistentes.Contains(nuevo.Clasificacion.Trim().ToLower()))
                        .Select(s => new Domain.Entities.DWH.Dimensions.Dim_Sentimiento { ID_Sentimiento = s.ID_Sentimiento, Clasificacion = s.Clasificacion.Trim().ToLower() })
                        .ToList();
                    if (nuevosSentimientos.Any()) await _context.Dim_Sentimiento.AddRangeAsync(nuevosSentimientos);
                }

                //Guardar dimensiones
                await _context.SaveChangesAsync();

                var fuentesMap = await _context.Dim_FuentesDatos
                    .GroupBy(f => f.ID_FuenteDatos.Trim().ToLower())
                    .ToDictionaryAsync(g => g.Key, g => g.Max(x => x.Key_FuenteDatos));

                var sentimientosMap = await _context.Dim_Sentimiento
                    .GroupBy(s => s.Clasificacion.Trim().ToLower())
                    .ToDictionaryAsync(g => g.Key, g => g.Max(x => x.Key_Sentimiento));

                var tiempoMap = await _context.Dim_Tiempo
                    .GroupBy(t => t.ID_Tiempo)
                    .ToDictionaryAsync(g => g.Key, g => g.Max(x => x.Key_Tiempo));

                var productosMap = await _context.Dim_Producto
                    .GroupBy(p => p.ID_Producto)
                    .ToDictionaryAsync(g => g.Key, g => g.Max(x => x.Key_Producto));

                var clientesMap = await _context.Dim_Clientes
                    .GroupBy(c => c.ID_Clientes)
                    .ToDictionaryAsync(g => g.Key, g => g.Max(x => x.Key_Clientes));

                var factEntities = new List<Fact_Opiniones>();

                if (data.Opiniones != null && data.Opiniones.Any())
                {
                    foreach (var dto in data.Opiniones)
                    {
                        //Fuente
                        string idFuenteBuscado = dto.ID_FuenteDatos?.Trim().ToLower() ?? "";
                        if (!fuentesMap.TryGetValue(idFuenteBuscado, out int fkFuente))
                        {
                            _Logger.LogWarning("FK_FuenteDatos no encontrada: '{id}', Omitiendo", idFuenteBuscado);
                            continue;
                        }

                        //Sentimiento
                        string clasifBuscada = dto.ClasificacionRaw?.Trim().ToLower() ?? "";
                        if (!sentimientosMap.TryGetValue(clasifBuscada, out int fkSentimiento))
                        {
                            _Logger.LogWarning("FK_Sentiemiento no encontrada: '{clas}', Omitiendo", dto.ClasificacionRaw);
                            continue;
                        }

                        //Tiempo
                        if (!int.TryParse(dto.FechaCarga.ToString("yyyyMMdd"), out int idTiempoBuscado)) continue;
                        if (!tiempoMap.TryGetValue(idTiempoBuscado, out int fkTiempo))
                        {
                            _Logger.LogWarning("FK_Tiempo no encontrada: '{id}', Omitiendo", idTiempoBuscado);
                            continue;
                        }

                        //Producto
                        if (!productosMap.TryGetValue(dto.FK_Producto, out int fkProducto))
                        {
                            _Logger.LogWarning("FK_Producto no encontrada (ID Negocio: {id}), Omitiendo", dto.FK_Producto);
                            continue;
                        }

                        //Cliente
                        if (!clientesMap.TryGetValue(dto.FK_Clientes, out int fkCliente))
                        {
                            _Logger.LogWarning("FK_Clientes no encontrada (ID Negocio: {id}), Omitiendo", dto.FK_Clientes);
                            continue;
                        }

                        //Mapeo final
                        factEntities.Add(new Fact_Opiniones
                        {
                            ID_Opiniones = dto.ID_Opiniones,
                            Comentario = dto.Comentario,
                            ClasificacionRaw = dto.ClasificacionRaw,
                            PalabrasClave = dto.PalabrasClave,
                            PuntajeSatisfaccion = dto.PuntajeSatisfaccion,
                            FechaCarga = dto.FechaCarga,
                            FK_Clientes = fkCliente,
                            FK_Producto = fkProducto,
                            FK_FuenteDatos = fkFuente,
                            FK_Sentiemiento = fkSentimiento,
                            FK_Tiempo = fkTiempo
                        });
                    }

                    if (factEntities.Any())
                    {
                        await _context.Fact_Opiniones.AddRangeAsync(factEntities);
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                _Logger.LogInformation("Proceso completado");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _Logger.LogError(ex, "Error al guardar en el Data Warehouse");
                throw;
            }
        }
    }
}