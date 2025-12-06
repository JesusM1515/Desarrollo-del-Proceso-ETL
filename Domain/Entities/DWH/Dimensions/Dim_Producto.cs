using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.DWH.Dimensions
{
    [Table("Dim_Producto", Schema = "Dimension")]
    public class Dim_Producto
    {
        [Key]
        public int Key_Producto { get; set; }
        public int ID_Producto { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public string Categoria { get; set; } = string.Empty;
    }
}
