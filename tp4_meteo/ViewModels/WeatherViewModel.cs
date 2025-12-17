using System.Collections.ObjectModel;
using System.Globalization; 
using System.Windows;       
using System.Windows.Input; 
using tp4_meteo.Data;
using tp4_meteo.Models;
using tp4_meteo.Services;
using tp4_meteo.Properties;
using tp4_meteo.ViewModels.Commands;

namespace tp4_meteo.ViewModels
{
    public class WeatherViewModel : ViewModelBase
    {
        private readonly IMeteoRepository _repository;
        private readonly IMeteoService _meteoService;
        private readonly IConfigService _configService;

        public ObservableCollection<Region> Regions { get; set; } = new ObservableCollection<Region>();
        public ObservableCollection<PrevisionData> Previsions { get; set; } = new ObservableCollection<PrevisionData>();

        private Region _selectedRegion;
        public Region SelectedRegion
        {
            get => _selectedRegion;
            set
            {
                if (Set(ref _selectedRegion, value))
                {
                    if (_selectedRegion != null)
                    {
                        LoadMeteoCommand.Execute(null);
                    }
                }
            }
        }

        private string _newNom;
        public string NewNom { get => _newNom; set => Set(ref _newNom, value); }

        private string _newLat;
        public string NewLat { get => _newLat; set => Set(ref _newLat, value); }

        private string _newLon;
        public string NewLon { get => _newLon; set => Set(ref _newLon, value); }

        public AsyncCommand LoadMeteoCommand { get; }
        public RelayCommand AddRegionCommand { get; }
        public RelayCommand DeleteRegionCommand { get; }

        // --- Constructeur ---
        public WeatherViewModel(IMeteoRepository repository, IMeteoService meteoService, IConfigService configService)
        {
            _repository = repository;
            _meteoService = meteoService;
            _configService = configService;

            LoadMeteoCommand = new AsyncCommand(async () =>
            {
                if (SelectedRegion == null) return;

                Previsions.Clear();

                string apiKey = _configService.ApiKey;
                if (string.IsNullOrWhiteSpace(apiKey))
                {
                    MessageBox.Show(Resources.ErrorApiKey, Resources.WarningTitle, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var data = await _meteoService.GetPrevisionsAsync(SelectedRegion.Latitude, SelectedRegion.Longitude, apiKey);

                if (data != null)
                {
                    foreach (var d in data) Previsions.Add(d);
                }
            });

            AddRegionCommand = new RelayCommand(_ =>
            {
               // Culture clavier
                string currentSep = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

                // remplace le point la virgule par le séparateur valide 
                string latClean = NewLat.Replace(".", currentSep).Replace(",", currentSep);
                string lonClean = NewLon.Replace(".", currentSep).Replace(",", currentSep);

                // conversion
                if (double.TryParse(latClean, out double lat) &&
                    double.TryParse(lonClean, out double lon) &&
                    !string.IsNullOrWhiteSpace(NewNom))
                {
                    if (lat < -90 || lat > 90 || lon < -180 || lon > 180)
                    {
                        MessageBox.Show("Latitude (-90 à 90) ou Longitude (-180 à 180) hors limites.",
                                        Resources.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    var r = new Region { Nom = NewNom, Latitude = lat, Longitude = lon };

                    _repository.AddRegion(r); // Sauvegarde bd
                    Regions.Add(r);          

                    NewNom = ""; NewLat = ""; NewLon = "";
                }
                else
                {
                    MessageBox.Show(Resources.ErrorInput, Resources.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });

            // supprimer
            DeleteRegionCommand = new RelayCommand(_ =>
            {
                if (SelectedRegion != null)
                {
                    var result = MessageBox.Show($"{Resources.BtnDelete} : {SelectedRegion.Nom} ?",
                                                 "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        _repository.DeleteRegion(SelectedRegion);
                        Regions.Remove(SelectedRegion);
                        SelectedRegion = null;
                        Previsions.Clear();
                    }
                }
            });

            // load depuis bd
            ChargerRegions();
        }

        private void ChargerRegions()
        {
            Regions.Clear();
            var list = _repository.GetAllRegions();
            foreach (var item in list) Regions.Add(item);
        }
    }
}