using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input; 
using tp4_meteo.Data;
using tp4_meteo.Models;
using tp4_meteo.Services;
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

        // region 
        private Region _selectedRegion;
        public Region SelectedRegion
        {
            get => _selectedRegion;
            set
            {
                if (Set(ref _selectedRegion, value))
                {
                    // changement region 
                    if (_selectedRegion != null)
                    {
                        LoadMeteoCommand.Execute(null);
                    }
                }
            }
        }

        // ajout
        private string _newNom; public string NewNom { get => _newNom; set => Set(ref _newNom, value); }
        private string _newLat; public string NewLat { get => _newLat; set => Set(ref _newLat, value); }
        private string _newLon; public string NewLon { get => _newLon; set => Set(ref _newLon, value); }

        // Commandes
        public AsyncCommand LoadMeteoCommand { get; }
        public RelayCommand AddRegionCommand { get; }
        public RelayCommand DeleteRegionCommand { get; }

        public WeatherViewModel(IMeteoRepository repository, IMeteoService meteoService, IConfigService configService)
        {
            _repository = repository;
            _meteoService = meteoService;
            _configService = configService;

            // chargement meteo 
            LoadMeteoCommand = new AsyncCommand(async () =>
            {
                if (SelectedRegion == null) return;

                // nettoyer list 
                Previsions.Clear();

                // cle
                string apiKey = _configService.ApiKey;
                if (string.IsNullOrWhiteSpace(apiKey))
                {
                    MessageBox.Show("Clé API manquante", "Attention");
                    return;
                }

                // Appel Service
                var data = await _meteoService.GetPrevisionsAsync(SelectedRegion.Latitude, SelectedRegion.Longitude, apiKey);

                if (data != null)
                {
                    foreach (var d in data) Previsions.Add(d);
                }
            });

            AddRegionCommand = new RelayCommand(_ =>
            {
                if (double.TryParse(NewLat.Replace(".", ","), out double lat) &&
                    double.TryParse(NewLon.Replace(".", ","), out double lon) &&
                    !string.IsNullOrWhiteSpace(NewNom))
                {
                    var r = new Region { Nom = NewNom, Latitude = lat, Longitude = lon };
                    _repository.AddRegion(r);
                    Regions.Add(r);
                    NewNom = ""; NewLat = ""; NewLon = ""; // Reset
                }
                else
                {
                    MessageBox.Show("Coordonnées invalides.", "Erreur");
                }
            });

            DeleteRegionCommand = new RelayCommand(_ =>
            {
                if (SelectedRegion != null)
                {
                    if (MessageBox.Show($"Supprimer {SelectedRegion.Nom} ?", "Confirmer", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        _repository.DeleteRegion(SelectedRegion);
                        Regions.Remove(SelectedRegion);
                        SelectedRegion = null;
                        Previsions.Clear();
                    }
                }
            });

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