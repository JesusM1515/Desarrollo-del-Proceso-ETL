using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.DWH.Dimensions
{
    [Table("Dim_Categoria", Schema = "Dimension")]
    public class Dim_Categoria
    {
        [Key]
        public int Key_Categoria { get; set; }
        public int ID_Categoria { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set;} = string.Empty;
    }
}
