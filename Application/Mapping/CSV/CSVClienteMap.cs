using Application.DTO;
using CsvHelper.Configuration;

namespace Application.Mapping.CSV
{
    public class CSVClienteMap : ClassMap<CSVClienteDTO>
    {
        public CSVClienteMap()
        {
            Map(m => m.ID_Clientes).Name("IdCliente");
            Map(m => m.Nombre).Name("Nombre");
            Map(m => m.Email).Name("Email");
        }
    }
}
