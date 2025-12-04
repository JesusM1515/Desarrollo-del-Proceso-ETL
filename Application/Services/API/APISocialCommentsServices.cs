using Application.DTO;
using Application.Interfaces;
using Application.Interfaces.IRepositories;
using Application.Mapping.API;
using Domain.Entities.API;
using Domain.Entities.Base;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.API
{
    public class APISocialCommentsServices : ISocialComments
    {
        private readonly IAPISocialCommentsRepository _socialCommentsRepository;
        private readonly ILogger<APISocialCommentsServices> _logger;
        private readonly APISocialCommentsMap _socialCommentsMap;

        public APISocialCommentsServices(IAPISocialCommentsRepository socialCommentsRepository, 
            ILogger<APISocialCommentsServices> logger, APISocialCommentsMap socialCommentsMap) 
        { 
            this._socialCommentsRepository = socialCommentsRepository;
            this._logger = logger;
            this._socialCommentsMap = socialCommentsMap;
        
        }
        public async Task<OperationResult<IEnumerable<SocialCommentsDTO>>> GetAllSocialComments()
        {
            _logger.LogInformation("Intentando traer todos los datos de Social Comments");

            var DatosSocialComments = await _socialCommentsRepository.GetAllDataAsync();

            if (DatosSocialComments == null || !DatosSocialComments.Any())
            {
                _logger.LogWarning("No se encontraron los datos de Social Comments en la base de datos");
                return OperationResult<IEnumerable<SocialCommentsDTO>>.Failure("No se encontraron los Social Comments");
            }

            //Mapeo de los datos
            var DatosSocialCommentsMap = DatosSocialComments.Select(e => _socialCommentsMap.commentsDTOMap(e)).ToList();

            _logger.LogInformation("Se obtuvieron los Social Comments con exito");

            return OperationResult<IEnumerable<SocialCommentsDTO>>.Success("Datos de Social Comments recuperados", DatosSocialCommentsMap);
        }
    }
}
