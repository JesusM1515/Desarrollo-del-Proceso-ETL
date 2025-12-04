using CsvHelper.Configuration;
using Domain.Entities.DWH.Dimensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapping.CSV
{
    public class CSVProductoMap : ClassMap<Dim_ProductoDTO>
    {
        public CSVProductoMap()
        {
            Map(m => m.ID_Producto).Name("IdProducto");
            Map(m => m.Nombre).Name("Nombre");
            Map(m => m.FK_Categoria).Name("Categoría");
        }
    }
}
