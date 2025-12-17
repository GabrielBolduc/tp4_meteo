using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using MeteoWPF.Models;

namespace MeteoWPF.Services
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
            if (apiKey == "DEMO" || string.IsNullOrEmpty(apiKey)) return GenerateMockData();

            string url = $"{BASE_URL}?lat={latitude}&lon={longitude}&key={apiKey}&days=7&lang=fr";

            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var weatherData = JsonSerializer.Deserialize<WeatherResponse>(json);
                return weatherData?.Data ?? new List<PrevisionData>();
            }
            catch (Exception)
            {
                return null;
            }
        }

        private List<PrevisionData> GenerateMockData()
        {
            var liste = new List<PrevisionData>();
            var today = DateTime.Now;
            for (int i = 0; i < 7; i++)
            {
                liste.Add(new PrevisionData
                {
                    Date = today.AddDays(i).ToString("yyyy-MM-dd"),
                    HighTemp = 20 + i,
                    LowTemp = 10 + i,
                    Weather = new WeatherDescription { Description = "Test", IconCode = "c01d" }
                });
            }
            return liste;
        }
    }
}