using System;
using MassTransit;

namespace RabbitMQConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(new Uri("rabbitmq://localhost/"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint("temperature_queue", e =>
                {
                    e.Consumer<TemperatureConsumer>();
                });
            });

            bus.Start();

            Console.WriteLine("Sıcaklık değerleri dinleniyor...");
            Console.WriteLine("Çıkmak için bir tuşa basın.");
            Console.ReadKey();

            bus.Stop();
        }
    }
}