using NetworksManagement.Core;
using NetworksManagement.Data;
using NetworksManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworksManagement.Infrastructure
{
    public class LocationsRepository : ILocationsRepository
    {
        private readonly ApplicationDbContext _context;
        public LocationsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Location location)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Location> GetAll()
        {
            var locations = _context.Locations.OrderBy(g => g.Name);

            return locations;
        }
    }
}
