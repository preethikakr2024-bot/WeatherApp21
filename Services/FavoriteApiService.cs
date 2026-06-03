using System.Net.Http.Json;
using BlazorApp21.Models;

namespace BlazorApp21.Services
{
    public class FavoriteApiService
    {
        private readonly HttpClient _http;
        private readonly string _baseUrl;

        public FavoriteApiService(HttpClient http, IConfiguration config)
        {
            _http = http;
            _baseUrl = config["ApiSettings:BaseUrl"] ?? "https://localhost:7000";
        }

        public async Task<bool> SaveFavorite(string userId, string city, string weather)
        {
            var favorite = new FavoriteModel
            {
                UserId = userId,
                City = city,
                FavoriteWeather = weather
            };

            var response = await _http.PostAsJsonAsync($"{_baseUrl}/api/favorite", favorite);
            return response.IsSuccessStatusCode;
        }

        public async Task<FavoriteModel?> GetFavorite(string userId)
        {
            try
            {
                var response = await _http.GetAsync($"{_baseUrl}/api/favorite/{userId}");
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadFromJsonAsync<FavoriteModel>();
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> ClearFavorite(string userId)
        {
            var response = await _http.DeleteAsync($"{_baseUrl}/api/favorite/{userId}");
            return response.IsSuccessStatusCode;
        }
    }
    public class FavoriteModel
    {
        public string? UserId { get; set; }
        public string? City { get; set; }
        public string? FavoriteWeather { get; set; }  // ← fixed
    }

}