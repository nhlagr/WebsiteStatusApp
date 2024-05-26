namespace WebsiteStatus
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private HttpClient client;
        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            client = new HttpClient();
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            client.Dispose();
            _logger.LogInformation("The service has been stopped...");
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                var result =await client.GetAsync("https://www.youtube.com");

                if (result.IsSuccessStatusCode)
                {
                    _logger.LogInformation("The website is up. Status code {StatusCode}",result.StatusCode);
                }
                else
                {
                    _logger.LogError("The website is down. Status code {StatusCode}",result.StatusCode);
                }

                //if (_logger.IsEnabled(LogLevel.Information))
                //{burasý hata verirse geri aç
                //}
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
    