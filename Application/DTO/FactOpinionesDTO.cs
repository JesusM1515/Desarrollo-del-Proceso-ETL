namespace Application.DTO
{
    public class FactOpinionesDTO
    {
        public int ID_Opiniones { get; set; }
        public string Comentario { get; set; } = string.Empty;
        public string ClasificacionRaw { get; set; } = string.Empty;
        public string PalabrasClave { get; set; } = string.Empty;
        public int PuntajeSatisfaccion { get; set; }
        public DateTime FechaCarga { get; set; }
        public int FK_Clientes { get; set; }
        public int FK_Producto { get; set; }

        //solo se usa para resolver el FK
        public string ID_FuenteDatos { get; set; } = string.Empty;
    }
}
