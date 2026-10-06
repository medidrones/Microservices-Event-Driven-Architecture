using Common.Core.Consumers;
using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace Ticketing.Query.Infrastructure.Consumers;

public class ConsumerHostedService : IHostedService
{
    private readonly ILogger<ConsumerHostedService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public ConsumerHostedService(IOptions<ConsumerConfig> config, ILogger<ConsumerHostedService> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("El Event Consumer service esta trabajando.");
        
        var topic = "KAFKA_TOPIC";

        using (IServiceScope scope = _serviceProvider.CreateScope())
        {
            var eventConsumer = scope.ServiceProvider.GetRequiredService<IEventConsumer>();
            
            Task.Run(() => eventConsumer.Consume(topic), cancellationToken);
        }
        
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("El servicio del consumer se detuvo.");
        
        return Task.CompletedTask;
    }
}