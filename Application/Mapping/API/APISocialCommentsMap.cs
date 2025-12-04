using Application.DTO;
using Domain.Entities.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapping.API
{
    public class APISocialCommentsMap
    {
        public SocialCommentsDTO commentsDTOMap(ESocialComments eSocialComments)
        {
            return new SocialCommentsDTO
            {
                IdComment = eSocialComments.IdComment,
                IdCliente = eSocialComments.IdCliente,
                IdProducto = eSocialComments.IdProducto,
                Fuente = eSocialComments.Fuente,
                Fecha = eSocialComments.Fecha,
                Comentario = eSocialComments.Comentario
            };
        }
    }
}
