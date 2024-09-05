using Confluent.Kafka;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Api.Services
{
    public class KafkaProducer : IDisposable
    {
        private readonly string _bootstrapServers;
        private readonly string _topic;
        private readonly IProducer<Null, string> _producer;

        public KafkaProducer(string bootstrapServers, string topic)
        {
            if (string.IsNullOrWhiteSpace(bootstrapServers))
                throw new ArgumentNullException(nameof(bootstrapServers), "Bootstrap servers cannot be null or empty.");

            if (string.IsNullOrWhiteSpace(topic))
                throw new ArgumentNullException(nameof(topic), "Topic cannot be null or empty.");

            _bootstrapServers = bootstrapServers;
            _topic = topic;

            var config = new ProducerConfig {
                BootstrapServers = _bootstrapServers,
                Acks = Acks.All,
                SecurityProtocol = SecurityProtocol.Plaintext,
            };
            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        public async Task SendHeartbeatAsync(bool isAlive)
        {
            var ip = NetworkUtils.GetLocalIpAddress();
            var status = isAlive ? "is alive" : "is dead";
            var message = new Message<Null, string> { Value = $"Service {ip} {status}" };

            try
            {
                var deliveryResult = await _producer.ProduceAsync(_topic, message);
                Console.WriteLine($"Delivered '{message.Value}' to '{deliveryResult.TopicPartitionOffset}'.");
            }
            catch (ProduceException<Null, string> e)
            {
                Console.WriteLine($"Delivery failed: {e.Error.Reason}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }

        public void Dispose()
        {
            _producer?.Dispose();
        }
    }
}
