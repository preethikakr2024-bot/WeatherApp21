namespace BlazorApp21.Models
{
    public class ForecastResponse
    {
        public List<ForecastItem>? List { get; set; }
    }
    public class ForecastItem
    {
        public MainInfo? Main { get; set; }
        public WeatherInfo[]? Weather { get; set; }
        public string? DtTxt { get; set; }
    }
}