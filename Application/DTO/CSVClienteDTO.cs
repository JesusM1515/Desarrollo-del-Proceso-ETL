using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class CSVClienteDTO
    {
        public int ID_Clientes { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int Edad { get; set; } = 0; //Valor por defecto
        public string Email { get; set; } = string.Empty;
        public string Pais { get; set; } = "Desconocido"; //Valor por defecto
        public string Ciudad { get; set; } = "Desconocida"; //Valor por defecto
        public string Tipo { get; set; } = "General"; //Valor por defecto
    }
}
