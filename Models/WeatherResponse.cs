namespace BlazorApp21.Models
{
    public class WeatherResponse
    {
        public string? Name { get; set; }   // ✅ THIS FIXES YOUR ERROR
        public MainInfo? Main { get; set; }
        public WeatherInfo[]? Weather { get; set; }
    }

    public class MainInfo
    {
        public float Temp { get; set; }
    }

    public class WeatherInfo
    {
        public string? Main { get; set; }
        public string? Description { get; set; }
    }
}