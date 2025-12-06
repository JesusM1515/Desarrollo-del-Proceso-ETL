using Domain.Entities.DWH.Dimensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class DimSourceDataDTO
    {
        public List<Dim_Clientes> Clientes { get; set; } = new List<Dim_Clientes>();
        public List<Dim_Producto> Productos { get; set; } = new List<Dim_Producto>();
        //public List<Dim_Categoria> Categorias { get; set; } = new List<Dim_Categoria>();
        public List<Dim_FuentesDatos> Fuentes { get; set; } = new List<Dim_FuentesDatos>();
        public List<Dim_Tiempo> Tiempo { get; set; } = new List<Dim_Tiempo>();
        public List<Dim_Sentimiento> Sentimientos { get; set; } = new List<Dim_Sentimiento>();
    }
}
