using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using tp4_meteo.Models;

namespace tp4_meteo.Services
{
    public class WeatherBitService : IMeteoService
    {
        private readonly HttpClient _httpClient;
        private const string BASE_URL = "https://www.weatherbit.io/api/weather-forecast-16-day";

        public WeatherBitService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<PrevisionData>> GetPrevisionsAsync(double latitude, double longitude, string apiKey)
        {
            string url = $"{BASE_URL}?lat={latitude}&lon={longitude}&key={apiKey}&days=7&lang=fr";
            try
            {
                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode) return null;

                var json = await response.Content.ReadAsStringAsync();
                var weatherData = JsonSerializer.Deserialize<WeatherResponse>(json);
                return weatherData?.Data ?? new List<PrevisionData>();
            }
            catch
            {
                return null;
            }
        }
    }
}