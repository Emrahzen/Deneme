using System;
using System.Threading.Tasks;
using Deneme.Models;
using MassTransit;

namespace RabbitMQConsumer
{
    public class TemperatureConsumer : IConsumer<DenemeClass>
    {
        public Task Consume(ConsumeContext<DenemeClass> context)
        {
            var temperature = context.Message.Heat;
            Console.WriteLine($"Yeni sıcaklık değeri alındı: {temperature}");
            return Task.CompletedTask;
        }
    }

}
