using CsvHelper.Configuration;
using Domain.Entities.DWH.Dimensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapping.CSV
{
    public class CSVFuenteMap : ClassMap<Dim_FuentesDatosDTO>
    {
        public CSVFuenteMap()
        {
            Map(m => m.ID_FuenteDatos).Name("IdFuente");
            Map(m => m.TipoFuenteDatos).Name("TipoFuente");
        }
    }
}
