using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class CSVFuentesDTO
    {
        public int ID_FuenteDatos { get; set; }
        public string TipoFuenteDatos { get; set; } = string.Empty;
        public string NombreFuenteDatos { get; set; } = "N/A"; // Faltante en CSV, valor por defecto
        public string Plataforma { get; set; } = "N/A"; // Faltante en CSV, valor por defecto
        public DateTime? FechaCarga { get; set; }
    }
}
