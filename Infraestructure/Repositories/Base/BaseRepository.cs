using Application.Interfaces.IRepositories;
using Infraestructure.BD.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repositories.Base
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        private readonly Context _context;
        private readonly ILogger<BaseRepository<TEntity>> _logger;
        private DbSet<TEntity> Entities {  get; set; }

        public BaseRepository(Context context, ILogger<BaseRepository<TEntity>> logger)
        {
            this._context = context;
            this._logger = logger;
            this.Entities = _context.Set<TEntity>();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllDataAsync()
        {
            _logger.LogInformation("Intentando traer todos los datos");
            return await Entities.ToListAsync();
        }
    }
}
