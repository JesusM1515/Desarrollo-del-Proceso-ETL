using Application.DTO;
using Application.Interfaces.IRepositories;
using Infraestructure.BD.Context;
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
            _Logger.LogInformation("Intentando guardar datos de dimensiones");

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
               /* if (data.Categorias != null && data.Categorias.Any())
                    await _context.Dim_Categoria.AddRangeAsync(data.Categorias);*/

                if (data.Fuentes != null && data.Fuentes.Any())
                    await _context.Dim_FuentesDatos.AddRangeAsync(data.Fuentes);

                if (data.Clientes != null && data.Clientes.Any())
                    await _context.Dim_Clientes.AddRangeAsync(data.Clientes);

                if (data.Productos != null && data.Productos.Any())
                    await _context.Dim_Producto.AddRangeAsync(data.Productos);

                if (data.Tiempo != null && data.Tiempo.Any())
                    await _context.Dim_Tiempo.AddRangeAsync(data.Tiempo);

                if (data.Sentimientos != null && data.Sentimientos.Any())
                    await _context.Dim_Sentimiento.AddRangeAsync(data.Sentimientos);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                _Logger.LogInformation("Dimensiones guardadas con exito");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _Logger.LogError(ex, "Error al guardar dimensiones");
                throw;
            }
        }
    }
}
