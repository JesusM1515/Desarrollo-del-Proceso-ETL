using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.DWH.Dimensions
{
    [Table("Dim_Clientes", Schema = "Dimension")]
    public class Dim_Clientes
    {
        [Key]
        public int Key_Clientes {  get; set; }
        public int ID_Clientes { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int Edad { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Pais { get; set; } = string.Empty;
        public string Ciudad { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
    }
}
