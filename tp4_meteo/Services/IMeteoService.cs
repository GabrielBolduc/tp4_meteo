using System.Collections.Generic;
using System.Threading.Tasks;
using tp4_meteo.Models;

namespace tp4_meteo.Services
{
    public interface IMeteoService
    {
        Task<List<PrevisionData>> GetPrevisionsAsync(double latitude, double longitude, string apiKey);
    }
}