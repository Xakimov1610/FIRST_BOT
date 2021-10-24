using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PrayerTime.DTO.CurrentTime;
using PrayerTime.Model;

namespace PrayerTime.Services
{
    public class CurrentTimeService
    {
        private readonly HttpClientService _httpService = new HttpClientService();
        private readonly ILogger<CurrentTimeService> _logger;
        public CurrentTimeService(ILogger<CurrentTimeService> logger)
        {
            _logger = logger;
        }
        public async Task<string> getCurrentTime(string timezone)
        {
            var currentTimeApi = $"https://api.aladhan.com/v1/currentTime?zone={timezone}";
            var result = await _httpService.GetObjectAsync<CurrentTime>(currentTimeApi);
            if(result.IsSuccess)
            {
                var settings = new JsonSerializerOptions()
                {
                    WriteIndented = true
                };
                _logger.LogInformation("Current time is successfully recieved.");
                return result.Data.Data;
            }
            else
            {
                _logger.LogCritical("We can't connect to API.");
                return null;
            }
        }
    }
}