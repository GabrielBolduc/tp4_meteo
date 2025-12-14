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

    // Une prévision pour un jour spécifique
    public class PrevisionData
    {
        [JsonPropertyName("valid_date")] // YYYY-MM-DD
        public string Date { get; set; }

        [JsonPropertyName("high_temp")]
        public double HighTemp { get; set; }

        [JsonPropertyName("low_temp")]
        public double LowTemp { get; set; }

        [JsonPropertyName("weather")]
        public WeatherDescription Weather { get; set; }
    }

    // Description du temps (icone, texte)
    public class WeatherDescription
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("icon")]
        public string IconCode { get; set; } 
    }
}