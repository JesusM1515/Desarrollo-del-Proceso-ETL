using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.DWH
{
    [Table("StFact_Opiniones", Schema = "dbo")]
    public class StFact_Opiniones
    {
        [Key]
        public int Key_Staging { get; set; }
        public int ID_Opiniones { get; set; }
        public string Comentario { get; set; } = string.Empty;
        public string Clasificacion_Negocio { get; set; } = string.Empty;
        public string PalabrasClave { get; set; } = string.Empty;
        public int PuntajeSatisfaccion { get; set; }
        public DateTime FechaCarga { get; set; }
        public int? FK_Clientes { get; set; }
        public int? FK_FuenteDatos { get; set; }
        public int? FK_Producto { get; set; }
        public int? FK_Sentiemiento { get; set; }
        public int? FK_Tiempo { get; set; }
        public int ID_Clientes_Negocio { get; set; }
        public string ID_FuenteDatos_Negocio { get; set; } = string.Empty;
        public int ID_Producto_Negocio { get; set; }
        public int ID_Tiempo_Negocio { get; set; }
    }
}
