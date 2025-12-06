using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.DWH.Dimensions
{
    [Table("Dim_FuentesDatos", Schema = "Dimension")]
    public class Dim_FuentesDatos
    {
        [Key]
        public int Key_FuenteDatos { get; set; }
        public string ID_FuenteDatos { get; set; } = string.Empty;
        public string TipoFuenteDatos { get; set; } = string.Empty;
        public string NombreFuenteDatos { get; set; } = string.Empty;
        public string Plataforma { get; set; } = string.Empty;
        public DateTime FechaCarga {  get; set; }
    }
}
