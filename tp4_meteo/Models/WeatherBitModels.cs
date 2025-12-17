using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MeteoWPF.Models
{
    public class WeatherResponse
    {
        [JsonPropertyName("data")]
        public List<PrevisionData> Data { get; set; }

        [JsonPropertyName("city_name")]
        public string CityName { get; set; }
    }

    public class PrevisionData
    {
        [JsonPropertyName("valid_date")]
        public string Date { get; set; }

        [JsonPropertyName("high_temp")]
        public double HighTemp { get; set; }

        [JsonPropertyName("low_temp")]
        public double LowTemp { get; set; }

        [JsonPropertyName("weather")]
        public WeatherDescription Weather { get; set; }
    }

    public class WeatherDescription
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("icon")]
        public string IconCode { get; set; }
    }
}