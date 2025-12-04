using Application.Interfaces;
using Application.Interfaces.IRepositories;
using CsvHelper.Configuration;
using Domain.Entities.Base;
using Domain.Entities.CSV;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.CSV
{
    public class CSVSurveysServices : ISurveys
    {
        private readonly IFileReaderRepository<ESurveys> _IReaderRepository; //Esta interfaz hereda del file reader
        private readonly ILogger<CSVSurveysServices> _Logger;
        private readonly IPathProvider _EsurveyPath;
        private readonly ClassMap<ESurveys> _classMap;
        //En el servicio se utiliza el worker service
        public CSVSurveysServices(IFileReaderRepository<ESurveys> readerRepository, ILogger<CSVSurveysServices> logger, 
            IPathProvider eSurveyPath, ClassMap<ESurveys> classMap) 
        { 
            _IReaderRepository = readerRepository;
            _Logger = logger;
            _EsurveyPath = eSurveyPath;
            _classMap = classMap;
        }
        //<IEnumerable<ESurveys>
        async Task<OperationResult<IEnumerable<ESurveys>>> ISurveys.GetSurveysAllAsync()
        {
            try
            {
                _Logger.LogInformation("Iniciando proceso de lecura de datos");

                var resultadoRead = await _IReaderRepository.ReadFileAsync(_EsurveyPath.GetCsvSurveys(), _classMap);

                if (resultadoRead == null || !resultadoRead.Any())
                {
                    _Logger.LogWarning("El archivo CSV no contiene datos o no se pudo leer correctamente");
                    return OperationResult<IEnumerable<ESurveys>>.Failure("No se pudieron obtener los datos del CSV Surveys");
                }

                //Limpieza de duplicados y comentarios vacios revisar
                var surveys = resultadoRead
                    .GroupBy(s => s.ID_Opiniones)
                    .Select(g => g.First()) //Elimina duplicados por IdOpinion
                    .Where(s => !string.IsNullOrWhiteSpace(s.Comentario)) //Elimina comentarios vacios
                    .ToList();

                //Limpieza de caracteres especiales en comentarios
               foreach (var s in surveys)
                {
                    s.Comentario = new string(s.Comentario
                        .Where(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c))
                        .ToArray());
                }

                //Normalizacion de fechas, productos y clientes
                foreach (var s in surveys)
                {
                    s.FechaCarga = s.FechaCarga.Date; //Normaliza fecha al dia
                    s.FK_Producto = Math.Abs(s.FK_Producto); //Convierte los id en su valor absoluto para evitar errores
                    s.FK_Clientes = Math.Abs(s.FK_Clientes);
                }

                //Clasificacion de opiniones en positivas, negativas y neutras
                foreach (var s in surveys)
                {
                    if (s.PuntajeSatisfaccion >= 4)
                        s.ClasificacionRaw = "Positiva";
                    else if (s.PuntajeSatisfaccion <= 2)
                        s.ClasificacionRaw = "Negativa";
                    else
                        s.ClasificacionRaw = "Neutra";
                }

                //Calculo de metricas positivas
                int totalComentarios = surveys.Count;
                double porcentajeSatisfaccion = surveys.Count > 0
                    ? surveys.Count(s => s.ClasificacionRaw == "Positiva") * 100.0 / totalComentarios
                    : 0;

                _Logger.LogInformation("El archivo CSV ha sido procesado correctamente");
                Console.WriteLine("");
                _Logger.LogInformation("Total comentarios: {totalComentarios}", totalComentarios);
                _Logger.LogInformation("Porcentaje satisfacción: {porcentajeSatisfaccion}%", porcentajeSatisfaccion);

                //Verificar que si se esta leyendo el archivo csv y que los filtros funcionan sin dar problemas
                Console.WriteLine("");
                _Logger.LogInformation("Registros leidos: {count}", resultadoRead.Count());
                _Logger.LogInformation("Despues de eliminar duplicados y vacios: {count}", surveys.Count);
                foreach (var confirmar in surveys)
                {
                    Console.WriteLine(confirmar);
                }

                return OperationResult<IEnumerable<ESurveys>>.Success("Archivo CSV Surveys procesado con exito", surveys);
            }
            catch (Exception ex)
            {
                _Logger.LogError($"Error al leer el archivo CSV {ex}");
                return OperationResult<IEnumerable<ESurveys>>.Failure($"Error al procesar el archivo CSV {ex}");
            }
        }
    }
}
