using Application.DTO;
using Domain.Entities.DWH.Dimensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IRepositories
{
    public interface IDWHRepository
    {
        Task SaveProcessedDimensionsAsync(
        IEnumerable<Dim_Categoria> categorias,
        IEnumerable<Dim_FuentesDatos> fuentes,
        IEnumerable<Dim_Clientes> clientes,
        IEnumerable<Dim_Producto> productos);
    }
}
