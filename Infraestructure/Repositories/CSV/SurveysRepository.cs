using Application.Interfaces.IRepositories;
using CsvHelper;
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
    public class SurveysRepository : IFileReaderRepository<ESurveys>, ISurveyRepository
    {
        private readonly string? _PathFile;
        private readonly IConfiguration _Configuration;
        private readonly ILogger<SurveysRepository> _Logger;

        public SurveysRepository(IConfiguration configuration, ILogger<SurveysRepository> logger) 
        { 
            this._Configuration = configuration;
            this._Logger = logger;
        }

        async Task<IEnumerable<ESurveys>> IFileReaderRepository<ESurveys>.ReadFileAsync(string filepath)
        {
            _Logger.LogInformation($"Leyendo archivo CSV con el path: {filepath}");
            List<ESurveys> listSurveys = new List<ESurveys>();
            try
            {
                using var reader = new StreamReader(filepath, Encoding.GetEncoding("ISO-8859-1"));
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

                await foreach (var datos in csv.GetRecordsAsync<ESurveys>())
                {
                    listSurveys.Add(datos);
                }
            }
            catch (Exception ex)
            {
              _Logger.LogError($"Error al leer el archivo CSV con el path {filepath} Exepcion: {ex}");
            }
            return listSurveys;
        }
    }
}
