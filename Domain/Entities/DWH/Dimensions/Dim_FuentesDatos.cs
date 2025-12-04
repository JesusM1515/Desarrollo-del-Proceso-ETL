using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.DWH.Dimensions
{
    [Table("Dim_FuentesDatos", Schema = "Dimension")]
    public class Dim_FuentesDatos
    {
        [Key]
        public int Key_FuenteDatos { get; set; }
        public int ID_FuenteDatos { get; set; }
        public string TipoFuenteDatos { get; set; } = string.Empty;
        public string NombreFuenteDatos { get; set; } = string.Empty;
        public string Plataforma { get; set; } = string.Empty;
    }
}
