using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using tp4_meteo.Models;

namespace tp4_meteo.Services
{
    public class WeatherBitService : IMeteoService
    {
        private readonly HttpClient _httpClient;
        private const string BASE_URL = "https://api.weatherbit.io/v2.0/forecast/daily";

        public WeatherBitService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<PrevisionData>> GetPrevisionsAsync(double latitude, double longitude, string apiKey)
        {
   
            // bug de culture anglis / fr
            string latStr = latitude.ToString(CultureInfo.InvariantCulture);
            string lonStr = longitude.ToString(CultureInfo.InvariantCulture);

            string url = $"{BASE_URL}?lat={latStr}&lon={lonStr}&key={apiKey}&days=7&lang=fr";

            try
            {
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Erreur API ({response.StatusCode}) :\n{errorContent}",
                                    "Erreur Météo", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var weatherData = JsonSerializer.Deserialize<WeatherResponse>(json, options);

                return weatherData?.Data ?? new List<PrevisionData>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur technique : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }
    }
}