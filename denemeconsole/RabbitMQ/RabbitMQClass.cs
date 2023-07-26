using System;
using System.Threading;
using System.Threading.Tasks;
using Deneme.Models;
using MassTransit;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace TemperatureProducer
{
        public class TempService
        {
            
            private CancellationToken _cancellationToken;
            private readonly IBusControl _bus;
            

            public TempService( CancellationToken cancellationToken, IBusControl bus)
            {

                _cancellationToken = cancellationToken;
                _bus = bus;
            }

            public async Task StartTemp()
            {
                await _bus.StartAsync();
             

                while (!_cancellationToken.IsCancellationRequested)
                {
                    Console.ReadLine();
                    var random = new Random();
                    var randomHeatValue = random.Next(-20, 51);

                    await _bus.Publish(new DenemeClass { Heat = randomHeatValue });
                  
                    Console.WriteLine($"Yeni sıcaklık gönderildi: {randomHeatValue}");
                }

                await _bus.StopAsync();
                
            }
        }
    }
      
 
