using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class SocialCommentsDTO
    {
        public string IdComment { get; set; } = string.Empty;
        public string IdCliente { get; set; } = string.Empty;
        public string IdProducto { get; set; } = string.Empty;
        public string Fuente { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string Comentario { get; set; } = string.Empty;
    }
}
