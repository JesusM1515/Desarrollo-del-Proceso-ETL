using Application.DTO;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapping.CSV
{
    public class CSVCategoriaMap : ClassMap<CSVCategoriaDTO>
    {
        public CSVCategoriaMap()
        {
            Map(m => m.ID_Categoria).Name("ID_Categoria");
            Map(m => m.Nombre).Name("Nombre");
            Map(m => m.Descripcion).Name("Descripcion");
        }
    }
}
