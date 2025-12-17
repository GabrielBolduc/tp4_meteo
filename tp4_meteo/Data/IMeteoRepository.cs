using System.Collections.Generic;
using tp4_meteo.Models;

namespace tp4_meteo.Data
{
    public interface IMeteoRepository
    {
        IEnumerable<Region> GetAllRegions();
        void AddRegion(Region region);
        void DeleteRegion(Region region);
    }
}