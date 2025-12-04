using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class CSVProductoDTO
    {
        public int ID_Producto { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Marca { get; set; } = "N/A"; // Faltante en CSV, valor por defecto
        public decimal Precio { get; set; } = 0.00M; // Faltante en CSV, valor por defecto
        public string Categoria { get; set; } = "N/A"; // Faltante en CSV, valor por defecto
        public int FK_Categoria { get; set; }
    }
}
