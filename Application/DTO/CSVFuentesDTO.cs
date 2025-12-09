namespace Application.DTO
{
    public class CSVFuentesDTO
    {
        public string ID_FuenteDatos { get; set; } = string.Empty;
        public string TipoFuenteDatos { get; set; } = string.Empty;
        public string NombreFuenteDatos { get; set; } = "N/A"; //Valor por defecto
        public string Plataforma { get; set; } = "N/A"; //Valor por defecto
        public DateTime FechaCarga { get; set; }
    }
}