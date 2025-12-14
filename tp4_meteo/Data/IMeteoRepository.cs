using System.Collections.Generic;
using MeteoWPF.Models;

namespace MeteoWPF.Data
{
    public interface IMeteoRepository
    {
        IEnumerable<Region> GetAllRegions();
        void AddRegion(Region region);
        void DeleteRegion(Region region);
    }
}