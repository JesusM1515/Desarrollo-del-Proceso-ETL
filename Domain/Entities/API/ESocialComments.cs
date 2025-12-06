using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Domain.Entities.API
{
    public class ESocialComments
    {
        [Key]
        public int Key_Comment {  get; set; }
        public string IdComment { get; set; } = string.Empty;
        public string IdCliente { get; set; } = string.Empty;
        public string IdProducto { get; set; } = string.Empty;
        public string Fuente { get; set; } = string.Empty;
        public DateTime Fecha {  get; set; }
        public string Comentario { get; set; } = string.Empty;
    }
}
