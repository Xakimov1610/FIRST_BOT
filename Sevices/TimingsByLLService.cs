using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PrayerTime.DTO.TimingsByLL;
using PrayerTime.Model;

namespace PrayerTime.Services
{
    public class TimingsByLLService
    {
        private readonly HttpClientService _httpService = new HttpClientService();
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
                _logger.LogInformation("Timing is successfully recieved.");
                return result;
            }
            else
            {
                _logger.LogCritical("We can't connect to API.");
                return null;
            }
        }
        public async Task<string> getTodayTimings(float longitude, float latitude)
        {
            var result = await _getResult("now", longitude, latitude);
            if(result.IsSuccess && result != null)
            {
                return ($"Bugungi namoz vaqtlari: {(result.Data.Data.Date.Gregorian.Date).Replace("-", ".")}\n",
                        $"Bomdod: {result.Data.Data.Timings.Fajr}\n",
                        $"Quyosh chiqishi: {result.Data.Data.Timings.Sunrise}\n",
                        $"Peshin: {result.Data.Data.Timings.Dhuhr}\n",
                        $"Asr: {result.Data.Data.Timings.Asr}\n",
                        $"Shom: {result.Data.Data.Timings.Maghrib}\n",
                        $"Xufton: {result.Data.Data.Timings.Isha}\n"
                        ).ToString().Replace(",", "").Replace("(", "").Replace(")", "");
            }
            else
            {
                return "We can't connect to API.";
            }
        }
        public async Task<string> getTomorrowTimings(float longitude, float latitude)
        {
            var result = await _getResult(DateTime.Now.AddDays(1).ToShortDateString().Replace("/", "-"), longitude, latitude);
            if(result.IsSuccess && result != null)
            {
                return ($"Ertangi namoz vaqtlari: {(result.Data.Data.Date.Gregorian.Date).Replace("-", ".")}\n",
                        $"Bomdod: {result.Data.Data.Timings.Fajr}\n",
                        $"Quyosh chiqishi: {result.Data.Data.Timings.Sunrise}\n",
                        $"Peshin: {result.Data.Data.Timings.Dhuhr}\n",
                        $"Asr: {result.Data.Data.Timings.Asr}\n",
                        $"Shom: {result.Data.Data.Timings.Maghrib}\n",
                        $"Xufton: {result.Data.Data.Timings.Isha}\n"
                        ).ToString().Replace(",", "").Replace("(", "").Replace(")", "");
            }
            else
            {
                return "We can't connect to API.";
            }
        }
    }
}