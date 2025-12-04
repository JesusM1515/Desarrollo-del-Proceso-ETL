using Application.Interfaces.IRepositories;
using Application.Mapping;
using CsvHelper;
using CsvHelper.Configuration;
using Domain.Entities.CSV;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repositories.CSV
{
    public class CSVRepository<TClass> : IFileReaderRepository<TClass> where TClass : class
    {
        private readonly ILogger<CSVRepository<TClass>> _Logger;

        public CSVRepository(ILogger<CSVRepository<TClass>> logger) 
        {
            this._Logger = logger;
        }

        public async Task<IEnumerable<TClass>> ReadFileAsync(string filepath, ClassMap classMap)
        {
            _Logger.LogInformation("Leyendo archivo con el path: {filepath}", filepath);
            List<TClass> listSurveys = new List<TClass>();

            try
            {
                using var reader = new StreamReader(filepath, Encoding.UTF8);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

                csv.Context.RegisterClassMap(classMap);

                await foreach (var datos in csv.GetRecordsAsync<TClass>())
                {
                    listSurveys.Add(datos);
                }
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex,"Error al leer el archivo con el path {filepath}", filepath);
            }

            _Logger.LogInformation("Datos leidos exitosamente");
            return listSurveys;
        }
    }
}
