using Application.Interfaces.IRepositories;
using Domain.Entities.API;
using Infraestructure.BD.Context;
using Infraestructure.Repositories.Base;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repositories.API
{
    public class APISocialCommentsRepository : BaseRepository<ESocialComments>, IAPISocialCommentsRepository
    {
        private readonly Context _context;
        private readonly ILogger<APISocialCommentsRepository> _logger;

        public APISocialCommentsRepository(Context context, ILogger<APISocialCommentsRepository> logger) : base(context, logger)
        {
            this._context = context;
            this._logger = logger;
        }

        public override async Task<IEnumerable<ESocialComments>> GetAllDataAsync()
        {
            _logger.LogInformation("Intentando traer todos los datos de Social Comments");
            return await base.GetAllDataAsync();
        }

    }
}
