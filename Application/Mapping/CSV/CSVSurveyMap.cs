using CsvHelper.Configuration;
using Domain.Entities.CSV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapping.CSV
{
    public class CSVSurveyMap : ClassMap<ESurveys>
    {
        public CSVSurveyMap() 
        {
            Map(m => m.ID_Opiniones).Name("IdOpinion");
            Map(m => m.FK_Clientes).Name("IdCliente");
            Map(m => m.FK_Producto).Name("IdProducto");
            Map(m => m.FechaCarga).Name("Fecha");
            Map(m => m.Comentario).Name("Comentario");
            Map(m => m.ClasificacionRaw).Name("Clasificación");
            Map(m => m.PuntajeSatisfaccion).Name("PuntajeSatisfacción");
            Map(m => m.Fuente).Name("Fuente");
        }
    }
}
