using Application.Interfaces;
using Application.Interfaces.IRepositories;
using Application.Mapping.CSV;
using Application.Services.CSV;
using CsvHelper.Configuration;
using Domain.Entities.CSV;
using Infraestructure.PathProvider;
using Infraestructure.Repositories.CSV;

namespace WorkerServiceDWH
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddScoped<ISurveys, CSVSurveysServices>();

            builder.Services.AddSingleton<ClassMap<ESurveys>, CSVSurveyMap>();

            builder.Services.AddScoped(typeof(IFileReaderRepository<>), typeof(CSVRepository<>));

            builder.Services.AddScoped<IPathProvider, CSVPaths>();

            builder.Services.AddHostedService<Worker>();

            var host = builder.Build();
            host.Run();
        }
    }
}