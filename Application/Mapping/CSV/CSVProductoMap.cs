using Application.DTO;
using CsvHelper.Configuration;

namespace Application.Mapping.CSV
{
    public class CSVProductoMap : ClassMap<CSVProductoDTO>
    {
        public CSVProductoMap()
        {
            Map(m => m.ID_Producto).Name("IdProducto");
            Map(m => m.Nombre).Name("Nombre");
            Map(m => m.Categoria).Name("Categoría");
        }
    }
}
