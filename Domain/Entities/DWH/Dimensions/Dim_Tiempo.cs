using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.DWH.Dimensions
{
    [Table("Dim_Tiempo", Schema = "Dimension")]
    public class Dim_Tiempo
    {
        [Key]
        public int Key_Tiempo { get; set; }
        public int ID_Tiempo { get; set; }
        public DateTime Fecha { get; set; }
        public string Dia { get; set; } = string.Empty;
        public string Mes { get; set; } = string.Empty;
        public int Year { get; set; }
        public int Trimestres { get; set; }
    }
}
