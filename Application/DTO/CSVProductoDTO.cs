namespace Application.DTO
{
    public class CSVProductoDTO
    {
        public int ID_Producto { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Marca { get; set; } = "N/A"; // Faltante en CSV, valor por defecto
        public decimal Precio { get; set; } = 0.00M; // Faltante en CSV, valor por defecto
        public string Categoria { get; set; } = string.Empty;
    }
}
