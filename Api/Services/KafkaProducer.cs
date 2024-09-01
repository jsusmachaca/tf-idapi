using Confluent.Kafka;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Api.Services
{
    public class KafkaProducer
    {
        private readonly string _bootstrapServers;
        private readonly string _topic;

        public KafkaProducer(string bootstrapServers, string topic)
        {
            _bootstrapServers = bootstrapServers;
            _topic = topic;
        }

        public async Task SendHeartbeatAsync(bool isAlive)
        {
            var config = new ProducerConfig { BootstrapServers = _bootstrapServers };

            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                var ip = NetworkUtils.GetLocalIpAddress();
                var status = isAlive ? "is alive" : "is dead";
                var message = new Message<Null, string> { Value = $"Service {ip} {status}" };
                
                try
                {
                    var deliveryResult = await producer.ProduceAsync(_topic, message);
                    Console.WriteLine($"Delivered '{message.Value}' to '{deliveryResult.TopicPartitionOffset}'.");
                }
                catch (ProduceException<Null, string> e)
                {
                    Console.WriteLine($"Delivery failed: {e.Error.Reason}");
                }
            }
        }
    }
}