using Domain.Entities.DWH.Dimensions;
using Domain.Entities.DWH.Facts;

namespace Application.DTO
{
    public class DimSourceDataDTO
    {
        //Dimensiones
        public List<Dim_Clientes> Clientes { get; set; } = new List<Dim_Clientes>();
        public List<Dim_Producto> Productos { get; set; } = new List<Dim_Producto>();
        public List<Dim_FuentesDatos> Fuentes { get; set; } = new List<Dim_FuentesDatos>();
        public List<Dim_Tiempo> Tiempo { get; set; } = new List<Dim_Tiempo>();
        public List<Dim_Sentimiento> Sentimientos { get; set; } = new List<Dim_Sentimiento>();

        //Facts
        public List<FactOpinionesDTO> Opiniones { get; set; } = new List<FactOpinionesDTO>();
    }
}
