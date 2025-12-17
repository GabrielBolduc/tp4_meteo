using System.IO;
using System.Text.Json;

namespace MeteoWPF.Services
{
    public class ConfigData
    {
        public string ApiKey { get; set; } = "DEMO";
        public string Language { get; set; } = "fr";
        public bool IsDarkTheme { get; set; }
    }

    public class RootConfig
    {
        public ConfigData AppSettings { get; set; }
    }

    public class ConfigService : IConfigService
    {
        private readonly string _filePath;
        private RootConfig _root;

        public ConfigService()
        {
            _filePath = "appsettings.json";
            Load();
        }

        public string ApiKey
        {
            get => _root.AppSettings.ApiKey;
            set => _root.AppSettings.ApiKey = value;
        }

        public string Language
        {
            get => _root.AppSettings.Language;
            set => _root.AppSettings.Language = value;
        }

        private void Load()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                _root = JsonSerializer.Deserialize<RootConfig>(json);
            }
            else
            {
                _root = new RootConfig { AppSettings = new ConfigData() };
            }
        }

        public void Save()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(_root, options);
            File.WriteAllText(_filePath, json);
        }
    }
}