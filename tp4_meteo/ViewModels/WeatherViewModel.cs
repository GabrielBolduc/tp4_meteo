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

        private Region _selectedRegion;
        public Region SelectedRegion
        {
            get => _selectedRegion;
            set
            {
                if (Set(ref _selectedRegion, value) && _selectedRegion != null)
                {
                    LoadMeteoCommand.Execute(null);
                }
            }
        }

        private string _newNom; public string NewNom { get => _newNom; set => Set(ref _newNom, value); }
        private string _newLat; public string NewLat { get => _newLat; set => Set(ref _newLat, value); }
        private string _newLon; public string NewLon { get => _newLon; set => Set(ref _newLon, value); }

        public AsyncCommand LoadMeteoCommand { get; }
        public RelayCommand AddRegionCommand { get; }
        public RelayCommand DeleteRegionCommand { get; }

        public WeatherViewModel(IMeteoRepository repository, IMeteoService meteoService, IConfigService configService)
        {
            _repository = repository;
            _meteoService = meteoService;
            _configService = configService;

            LoadMeteoCommand = new AsyncCommand(async () =>
            {
                if (SelectedRegion == null) return;
                Previsions.Clear();
                var data = await _meteoService.GetPrevisionsAsync(SelectedRegion.Latitude, SelectedRegion.Longitude, _configService.ApiKey);
                if (data != null) foreach (var d in data) Previsions.Add(d);
            });

            AddRegionCommand = new RelayCommand(_ => {
                if (double.TryParse(NewLat, out double lat) && double.TryParse(NewLon, out double lon) && !string.IsNullOrWhiteSpace(NewNom))
                {
                    var r = new Region { Nom = NewNom, Latitude = lat, Longitude = lon };
                    _repository.AddRegion(r);
                    Regions.Add(r);
                    NewNom = NewLat = NewLon = "";
                }
            });

            DeleteRegionCommand = new RelayCommand(_ => {
                if (SelectedRegion != null)
                {
                    _repository.DeleteRegion(SelectedRegion);
                    Regions.Remove(SelectedRegion);
                    SelectedRegion = null;
                    Previsions.Clear();
                }
            });

            foreach (var r in _repository.GetAllRegions()) Regions.Add(r);
        }
    }
}