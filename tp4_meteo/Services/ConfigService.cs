using System.IO;
using System.Text.Json;

namespace tp4_meteo.Services
{
    public class ConfigData
    {
        public string ApiKey { get; set; } = "";
        public string Language { get; set; } = "fr";
    }

    public class RootConfig
    {
        public ConfigData AppSettings { get; set; }
    }

    public class ConfigService : IConfigService
    {
        private readonly string _folderPath;
        private readonly string _userFilePath;
        private readonly string _defaultFilePath = "appsettings.json";
        private RootConfig _root;

        public ConfigService()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            _folderPath = Path.Combine(appData, "MeteoTP4");
            _userFilePath = Path.Combine(_folderPath, "user_config.json");

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
            // empeche le crash dans l'installeur
            if (File.Exists(_userFilePath))
            {
                try
                {
                    var json = File.ReadAllText(_userFilePath);
                    _root = JsonSerializer.Deserialize<RootConfig>(json);
                }
                catch { }
            }

            if (_root == null && File.Exists(_defaultFilePath))
            {
                try
                {
                    var json = File.ReadAllText(_defaultFilePath);
                    _root = JsonSerializer.Deserialize<RootConfig>(json);
                }
                catch { }
            }

            if (_root == null)
            {
                _root = new RootConfig { AppSettings = new ConfigData() };
            }
        }

        public void Save()
        {
            try
            {
                if (!Directory.Exists(_folderPath))
                {
                    Directory.CreateDirectory(_folderPath);
                }

                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(_root, options);

                
                File.WriteAllText(_userFilePath, json);
            }
            catch (Exception)
            {
                // empeche le crash 
            }
        }
    }
}