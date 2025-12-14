using System.Collections.Generic;
using System.Threading.Tasks;
using MeteoWPF.Models;

namespace MeteoWPF.Services
{
    public interface IMeteoService
    {
        // On demande une liste de prévisions pour une latitude/longitude donnée
        Task<List<PrevisionData>> GetPrevisionsAsync(double latitude, double longitude, string apiKey);
    }
}