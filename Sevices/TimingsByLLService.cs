using System;
using System.Runtime.Serialization.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PrayerTime.DTO.TimingsByLL;
using PrayerTime.Model;

namespace PrayerTime.Services
{
    public class TimingsByLLService
    {
        private readonly HttpClientService _httpService;
        private readonly ILogger<TimingsByLLService> _logger;
        public TimingsByLLService(ILogger<TimingsByLLService> logger)
        {
            _logger = logger;
        }
        private async Task<HttpResult<TimingsByLL>> _getResult(string time, float longitude, float latitude)
        {
            var timingsByLLApi = $"https://api.aladhan.com/v1/timings/{time}?latitude={latitude}&longitude={longitude}&method=14&school=1";
            var result = await _httpService.GetObjectAsync<TimingsByLL>(timingsByLLApi);
            if(result.IsSuccess)
            {
                var settings = new JsonSerializerOptions()
                {
                    WriteIndented = true
                };
                _logger.LogInformation("Result is successfully recieved.");
                return result;
            }
            else
            {
                _logger.LogInformation("We can't connect to API.");
                return null;
            }
        }
        public async Task<string> getTodayTimings(float longitude, float latitude)
        {
            var result = await _getResult("now", longitude, latitude);
            if(result.IsSuccess && result != null)
            {
                Console.WriteLine($"Bomdod: {result.Data.Data.Timings.Fajr}\nQuyosh chiqishi: {result.Data.Data.Timings.Sunrise}\n");
                return $"Bomdod: {result.Data.Data.Timings.Fajr}\nQuyosh chiqishi: {result.Data.Data.Timings.Sunrise}\n";
            }
            else
            {
                return "We can't connect to API.";
            }
        }
    }
}