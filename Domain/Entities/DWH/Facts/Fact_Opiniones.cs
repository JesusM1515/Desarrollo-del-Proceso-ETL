using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.DWH.Facts
{
    [Table("Fact_Opiniones", Schema = "Fact")]
    public class Fact_Opiniones
    {
        [Key]
        public int Key_Opiniones { get; set; }
        public string ID_Opiniones { get; set; } = string.Empty;
        public string Comentario { get; set; } = string.Empty;
        public string ClasificacionRaw { get; set; } = string.Empty;
        public string PalabrasClave { get; set; } = string.Empty;
        public int PuntajeSatisfaccion { get; set; }
        public DateTime FechaCarga { get; set; }
        public int FK_Clientes { get; set; }
        public int FK_FuenteDatos { get; set; }
        public int FK_Producto { get; set; }
        public int FK_Sentiemiento { get; set; }
        public int FK_Tiempo { get; set; }
    }
}
