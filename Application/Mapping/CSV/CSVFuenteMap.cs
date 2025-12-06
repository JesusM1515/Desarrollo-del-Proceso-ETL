using Application.DTO;
using CsvHelper.Configuration;

namespace Application.Mapping.CSV
{
    public class CSVFuenteMap : ClassMap<CSVFuentesDTO>
    {
        public CSVFuenteMap()
        {
            Map(m => m.ID_FuenteDatos).Name("IdFuente");
            Map(m => m.TipoFuenteDatos).Name("TipoFuente");
            Map(m => m.FechaCarga).Name("FechaCarga");
        }
    }
}
