using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.DWH.Dimensions
{
    [Table("Dim_Sentimiento", Schema = "Dimension")]
    public class Dim_Sentimiento
    {
        [Key]
        public int Key_Sentimiento { get; set; }
        public int ID_Sentimiento { get; set; }
        public string Clasificacion { get; set; } = string.Empty;
    }
}
