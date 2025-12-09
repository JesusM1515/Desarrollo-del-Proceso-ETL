using Application.DTO;
using Application.Interfaces;
using Application.Interfaces.IRepositories;
using Application.Mapping.CSV;
using Application.Services.CSV;
using Application.Services.DWH;
using CsvHelper.Configuration;
using Domain.Entities.CSV;
using Infraestructure.BD.Context;
using Infraestructure.PathProvider;
using Infraestructure.Repositories.CSV;
using Infraestructure.Repositories.DWH;

namespace WorkerServiceDWH
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddSqlServer<DWHContext>(builder.Configuration.GetConnectionString("AppConnection"));

            builder.Services.AddScoped<ISurveys, CSVSurveysServices>();

            builder.Services.AddSingleton<ClassMap<CSVSurveyDTO>, CSVSurveyMap>();
            builder.Services.AddSingleton<ClassMap<CSVProductoDTO>, CSVProductoMap>();
            builder.Services.AddSingleton<ClassMap<CSVFuentesDTO>, CSVFuenteMap>();
            builder.Services.AddSingleton<ClassMap<CSVClienteDTO>, CSVClienteMap>();

            builder.Services.AddScoped(typeof(IFileReaderRepository<>), typeof(CSVRepository<>)); //type por que es generico

            builder.Services.AddScoped<IPathProvider, CSVPaths>();

            builder.Services.AddScoped<IDWHRepository, DWHRepository>();

            builder.Services.AddScoped<IDWHHandlerServices, DWHHandlerServices>();

            builder.Services.AddScoped<IPathProvider, CSVPaths>();

            builder.Services.AddHostedService<Worker>();

            var host = builder.Build();
            host.Run();
        }
    }
}