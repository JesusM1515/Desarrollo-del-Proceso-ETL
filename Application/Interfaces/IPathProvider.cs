using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPathProvider
    {
        public string GetCsvSurveys();
        public string GetCsvClientes();
        public string GetCsvFuentes();
        public string GetCsvProductos();
    }
}
