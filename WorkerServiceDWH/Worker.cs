using Application.Interfaces;

namespace WorkerServiceDWH
{
    public class Worker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<Worker> _logger;
        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var surveysService = scope.ServiceProvider.GetRequiredService<ISurveys>();

                        var resultado = await surveysService.GetSurveysAllAsync();

                        if (resultado.IsSuccess)
                        {
                            _logger.LogInformation("Surveys procesados correctamente: {msg}", resultado.Message);
                        }
                        else
                        {
                            _logger.LogWarning("Error procesando surveys: {msg}", resultado.Message);
                        }
                    }
                
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
