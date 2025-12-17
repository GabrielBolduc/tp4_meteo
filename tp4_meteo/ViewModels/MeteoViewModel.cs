using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using MeteoWPF.Data;
using MeteoWPF.Models;
using MeteoWPF.Services;

namespace MeteoWPF.ViewModels
{
    public class MeteoViewModel : ViewModelBase
    {
        private readonly IMeteoRepository _repository;
        private readonly IMeteoService _meteoService;
        private readonly IConfigService _configService;

        public ObservableCollection<Region> Regions { get; set; }

        private ObservableCollection<PrevisionData> _previsions;
        public ObservableCollection<PrevisionData> Previsions
        {
            get => _previsions;
            set { _previsions = value; OnPropertyChanged(); }
        }

        private Region _selectedRegion;
        public Region SelectedRegion
        {
            get => _selectedRegion;
            set
            {
                _selectedRegion = value;
                OnPropertyChanged();
                if (_selectedRegion != null) LoadMeteoCommand.Execute(null);
            }
        }

        public ICommand LoadMeteoCommand { get; }

        public MeteoViewModel(IMeteoRepository repository, IMeteoService meteoService, IConfigService configService)
        {
            _repository = repository;
            _meteoService = meteoService;
            _configService = configService;

            Regions = new ObservableCollection<Region>();
            Previsions = new ObservableCollection<PrevisionData>();
            LoadMeteoCommand = new RelayCommand(async _ => await ChargerMeteoAsync());

            ChargerRegionsDeLaBD();
        }

        private void ChargerRegionsDeLaBD()
        {
            var regionsDb = _repository.GetAllRegions();
            Regions.Clear();
            foreach (var r in regionsDb) Regions.Add(r);
        }

        private async Task ChargerMeteoAsync()
        {
            if (SelectedRegion == null) return;
            var data = await _meteoService.GetPrevisionsAsync(
                SelectedRegion.Latitude, SelectedRegion.Longitude, _configService.ApiKey);

            Previsions.Clear();
            if (data != null)
            {
                foreach (var item in data) Previsions.Add(item);
            }
        }
    }
}