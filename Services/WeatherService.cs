using BlazorApp21.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorApp21.Services
{
    public class WeatherService
    {
        private readonly HttpClient http;
        private readonly string apiKey = "0c96d91b611fac9219a7be00818067bb";

        public WeatherService(HttpClient http)
        {
            this.http = http;
        }

        // ✅ CURRENT WEATHER
        public async Task<WeatherResponse?> GetWeather(string city)
        {
            var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric";

            var response = await http.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<WeatherResponse>();
        }

        // ✅ 5-DAY FORECAST (NEW)
        public async Task<ForecastResponse?> GetForecast(string city)
        {
            var url = $"https://api.openweathermap.org/data/2.5/forecast?q={city}&appid={apiKey}&units=metric";

            var response = await http.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<ForecastResponse>();
        }
    }
}