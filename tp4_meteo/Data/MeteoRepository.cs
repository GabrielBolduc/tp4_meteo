using System.Collections.Generic;
using System.Linq;
using MeteoWPF.Models;

namespace MeteoWPF.Data
{
    public class MeteoRepository : IMeteoRepository
    {
        private readonly MeteoDbContext _context;

        public MeteoRepository(MeteoDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Region> GetAllRegions()
        {
            return _context.Regions.ToList();
        }

        public void AddRegion(Region region)
        {
            _context.Regions.Add(region);
            _context.SaveChanges();
        }

        public void DeleteRegion(Region region)
        {
            if (_context.Regions.Contains(region))
            {
                _context.Regions.Remove(region);
                _context.SaveChanges();
            }
            else
            {
                _context.Regions.Attach(region);
                _context.Regions.Remove(region);
                _context.SaveChanges();
            }
        }
    }
}