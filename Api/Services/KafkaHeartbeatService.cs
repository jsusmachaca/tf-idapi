namespace Api.Services
{
    public class KafkaHeartbeatService : BackgroundService
    {
        private readonly KafkaProducer _kafkaProducer;
        private readonly ILogger<KafkaHeartbeatService> _logger;
        private readonly TimeSpan _interval = TimeSpan.FromSeconds(30);

        public KafkaHeartbeatService(KafkaProducer kafkaProducer, ILogger<KafkaHeartbeatService> logger)
        {
            _kafkaProducer = kafkaProducer;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _kafkaProducer.SendHeartbeatAsync(true);
                    _logger.LogInformation("Heartbeat sent at: {time}", DateTimeOffset.Now);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error sending heartbeat.");
                }

                await Task.Delay(_interval, stoppingToken);
            }
        }
    }
}
