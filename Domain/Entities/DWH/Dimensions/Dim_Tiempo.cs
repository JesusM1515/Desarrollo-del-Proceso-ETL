using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.DWH.Dimensions
{
    public class Dim_Tiempo
    {
        public int Key_Tiempo { get; set; }
        public int ID_Tiempo { get; set; }
        public DateTime Fecha { get; set; }
        public string Dia { get; set; } = string.Empty;
        public string Mes { get; set; } = string.Empty;
        public int Anio { get; set; }
        public int Trimestres { get; set; }
    }
}
