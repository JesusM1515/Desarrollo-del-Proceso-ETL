using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.PathProvider
{
    public class CSVPaths : IPathProvider
    {
        string IPathProvider.GetCsvSurveys()
        {
            return "C:\\Users\\jesus\\OneDrive\\Escritorio\\CsvOpiniones\\Archivo CSV Análisis de Opiniones de Clientes-20251024\\surveys_part1.csv"; 
        }
        string IPathProvider.GetCsvClientes()
        {
            return "C:\\Users\\jesus\\OneDrive\\Escritorio\\CsvOpiniones\\Archivo CSV Análisis de Opiniones de Clientes-20251024\\clients.csv";
        }
        string IPathProvider.GetCsvFuentes()
        {
            return "C:\\Users\\jesus\\OneDrive\\Escritorio\\CsvOpiniones\\Archivo CSV Análisis de Opiniones de Clientes-20251024\\fuente_datos.csv";
        }
        string IPathProvider.GetCsvProductos()
        {
            return "C:\\Users\\jesus\\OneDrive\\Escritorio\\CsvOpiniones\\Archivo CSV Análisis de Opiniones de Clientes-20251024\\products.csv";
        }
    }
}
