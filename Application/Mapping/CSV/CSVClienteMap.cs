using CsvHelper.Configuration;
using Domain.Entities.CSV;
using Domain.Entities.DWH.Dimensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapping.CSV
{
    public class CSVClienteMap : ClassMap<Dim_ClientesDTO>
    {
        public CSVClienteMap()
        {
            Map(m => m.ID_Clientes).Name("IdCliente");
            Map(m => m.Nombre).Name("Nombre");
            Map(m => m.Email).Name("Email");
        }
    }
}
