using Application.DTO;
using Application.Interfaces;
using Application.Interfaces.IRepositories;
using Application.Services.CSV;
using CsvHelper.Configuration;
using Domain.Entities.CSV;
using Domain.Entities.DWH.Dimensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task SaveProcessedDimensionsAsync(
            IEnumerable<Dim_Categoria> categorias,
            IEnumerable<Dim_FuentesDatos> fuentes,
            IEnumerable<Dim_Clientes> clientes,
            IEnumerable<Dim_Producto> productos)
        {
            _Logger.LogInformation("Intentando guardar datos de dimensiones");

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.Dim_Categoria.AddRangeAsync(categorias);
                await _context.Dim_FuentesDatos.AddRangeAsync(fuentes);
                await _context.Dim_Clientes.AddRangeAsync(clientes);
                await _context.Dim_Producto.AddRangeAsync(productos);

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
