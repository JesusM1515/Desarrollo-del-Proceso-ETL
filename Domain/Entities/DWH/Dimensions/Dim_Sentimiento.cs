using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.DWH.Dimensions
{
    public class Dim_Sentimiento
    {
        public int Key_Sentimiento { get; set; }
        public int ID_Sentimiento { get; set; }
        public string Clasificacion { get; set; } = string.Empty;
    }
}
