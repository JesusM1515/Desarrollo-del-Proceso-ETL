using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.CSV
{
    public class ESurveys
    {
        [Key]
        public int Key_Opiniones { get; set; }
        public int ID_Opiniones { get; set; }
        public int FK_Clientes { get; set; }
        public int FK_Producto { get; set; }
        public DateTime FechaCarga { get; set; }
        public string Comentario { get; set; } = string.Empty;
        public string ClasificacionRaw { get; set; } = string.Empty;
        public int PuntajeSatisfaccion { get; set; }
        public string Fuente { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"OpinionId: {ID_Opiniones}, ClienteId: {FK_Clientes}, ProductoId: {FK_Producto}, Fecha: {FechaCarga}, Clasificación: {ClasificacionRaw}, Puntaje: {PuntajeSatisfaccion}, Fuente: {Fuente}, Comentario: {Comentario}";
        }
    }
}
