using Deneme.Models;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Deneme.ApplicationDBContext;
using System;
namespace Deneme.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly AppDBContext _context;
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IBus _bus;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IBus bus, AppDBContext context)
        
        {
            _logger = logger;
            _context = context;
            _bus = bus;
        }

        //[HttpGet(Name = "GetWeatherForecast")]
        //public IEnumerable<WeatherForecast> Get()
        //{
        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = DateTime.Now.AddDays(index),
        //        TemperatureC = Random.Shared.Next(-20, 55),
        //        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}

        [HttpGet("data")]
        public IActionResult GetData()
        {
            var data = _context.denemeClass.ToList(); // Veritabanından verileri çekin
            return Ok(data);
        }
        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] DenemeClass message)
        {
            try
            {
                var random = new Random();
                var randomHeatValue = random.Next(-20, 51);
                // RabbitMQ'ya gönderilecek mesajı belirt
                var messageToSend = new DenemeClass
                {
                    Id = message.Id,
                    Heat = randomHeatValue
                 };

                // Mesajı RabbitMQ'ya yayınla
                await _bus.Publish(messageToSend);
                

                return Ok("Mesaj gönderildi.");
            }
            catch (System.Exception ex)
            {
                // Hata durumunda geri bildirim
                return BadRequest($"Hata oluştu: {ex.Message}");
            }
        }
    
}
}