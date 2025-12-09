namespace Application.DTO
{
    public class CSVSurveyDTO
    {
        public int ID_Opiniones { get; set; } //
        public string Comentario { get; set; } = string.Empty; //
        public string ClasificacionRaw { get; set; } = string.Empty; //
        public string PalabrasClave { get; set; } = string.Empty;
        public int PuntajeSatisfaccion { get; set; } //
        public DateTime FechaCarga { get; set; } //
        public int FK_Clientes { get; set; }//
        public int FK_Producto { get; set; }//
        public string Fuente { get; set; } = string.Empty;//Se utiliza para el servicio que lee el csv y lo muestra en consola
    }
}
