using System.Collections.Generic;

namespace BlazorApp21.Models
{
    public class UserModel
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;  // ✅ fixed CS8618
        public string FavoriteCity { get; set; } = string.Empty;
        public string FavoriteWeather { get; set; } = string.Empty;
        public List<string> FiveDayForecast { get; set; } = new List<string>();
    }
}