using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace tp4_meteo.Models
{

    public class WeatherResponse
    {
        [JsonPropertyName("data")]
        public List<PrevisionData> Data { get; set; }

        [JsonPropertyName("city_name")]
        public string CityName { get; set; }

        [JsonPropertyName("country_code")]
        public string CountryCode { get; set; }
    }

   // jour prevision
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

    // pour description mais pas utilser dans mes views 
    public class WeatherDescription
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("icon")]
        public string IconCode { get; set; }

        public string IconUrl => $"https://www.weatherbit.io/static/img/icons/{IconCode}.png";
    }
}