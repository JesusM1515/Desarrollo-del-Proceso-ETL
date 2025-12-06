using Application.DTO;
using CsvHelper.Configuration;

namespace Application.Mapping.CSV
{
    public class CSVCategoriaMap : ClassMap<CSVCategoriaDTO>
    {
        public CSVCategoriaMap()
        {
            Map(m => m.Nombre).Name("Categoría");
        }
    }
}
