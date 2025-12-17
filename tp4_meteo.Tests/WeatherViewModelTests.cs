using System.Collections.Generic;
using System.Threading.Tasks;
using Moq; // Pour les Mocks
using Xunit; // Pour les tests [Fact]
using tp4_meteo.Data;
using tp4_meteo.Models;
using tp4_meteo.Services;
using tp4_meteo.ViewModels;

namespace tp4_meteo.Tests
{
    public class WeatherViewModelTests
    {
        private readonly Mock<IMeteoRepository> _mockRepo;
        private readonly Mock<IMeteoService> _mockMeteoService;
        private readonly Mock<IConfigService> _mockConfigService;

        private readonly WeatherViewModel _viewModel;

        public WeatherViewModelTests()
        {
            _mockRepo = new Mock<IMeteoRepository>();
            _mockMeteoService = new Mock<IMeteoService>();
            _mockConfigService = new Mock<IConfigService>();

            _mockConfigService.Setup(c => c.ApiKey).Returns("FAKE_KEY_FOR_TESTS");

            _mockRepo.Setup(r => r.GetAllRegions()).Returns(new List<Region>());

            _viewModel = new WeatherViewModel(_mockRepo.Object, _mockMeteoService.Object, _mockConfigService.Object);
        }


        [Fact]
        public void Constructeur_DoitChargerRegions_DepuisLeRepository()
        {
            var regionsTest = new List<Region>
            {
                new Region { Nom = "Ville A", Latitude = 10, Longitude = 10 },
                new Region { Nom = "Ville B", Latitude = 20, Longitude = 20 }
            };

            _mockRepo.Setup(r => r.GetAllRegions()).Returns(regionsTest);

            var vm = new WeatherViewModel(_mockRepo.Object, _mockMeteoService.Object, _mockConfigService.Object);

            Assert.Equal(2, vm.Regions.Count); 
            Assert.Equal("Ville A", vm.Regions[0].Nom);
        }

        [Fact]
        public void AddRegionCommand_DonneesValides_DoitAjouterEtSauvegarder()
        {
            _viewModel.NewNom = "Sherbrooke";
            _viewModel.NewLat = "45.4";
            _viewModel.NewLon = "-71.9";

            _viewModel.AddRegionCommand.Execute(null);

            Assert.Single(_viewModel.Regions);
            Assert.Equal("Sherbrooke", _viewModel.Regions[0].Nom);

            _mockRepo.Verify(r => r.AddRegion(It.IsAny<Region>()), Times.Once);
        }

        [Fact]
        public void AddRegionCommand_DonneesInvalides_NeDoitPasSauvegarder()
        {
            _viewModel.NewNom = "Nowhere";
            _viewModel.NewLat = "PasUnNombre";
            _viewModel.NewLon = "-71.9";

            _viewModel.AddRegionCommand.Execute(null);

            Assert.Empty(_viewModel.Regions); // Rien ajouté
            _mockRepo.Verify(r => r.AddRegion(It.IsAny<Region>()), Times.Never); 
        }

        [Fact]
        public async Task LoadMeteoCommand_DoitRemplirPrevisions()
        {
            _viewModel.SelectedRegion = new Region { Nom = "Montreal", Latitude = 45, Longitude = -73 };

            var fakeData = new List<PrevisionData>
            {
                new PrevisionData { HighTemp = 25 },
                new PrevisionData { HighTemp = 22 }
            };

            _mockMeteoService
                .Setup(s => s.GetPrevisionsAsync(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<string>()))
                .ReturnsAsync(fakeData);

            await _viewModel.LoadMeteoCommand.ExecuteAsync();

            Assert.Equal(2, _viewModel.Previsions.Count);
            Assert.Equal(25, _viewModel.Previsions[0].HighTemp);
        }
    }
}