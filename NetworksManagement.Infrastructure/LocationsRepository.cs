using Microsoft.EntityFrameworkCore;
using NetworksManagement.Core;
using NetworksManagement.Data;
using NetworksManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworksManagement.Infrastructure
{
    public class LocationsRepository : ILocationsRepository
    {
        private readonly ApplicationDbContext _context;
        public LocationsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Location location)
        {
            _context.Add(location);

            await _context.SaveChangesAsync();
        }

        public bool Any(int locationId)
        {
            return _context.Locations.Any(l => l.Id == locationId);
        }

        public IQueryable<Location> GetAll()
        {
            var locations = _context.Locations.OrderBy(g => g.Name);

            return locations;
        }

        public async Task<Location> GetAsync(int? locationId)
        {
            var group = await _context.Locations.FirstOrDefaultAsync(l => l.Id == locationId);

            return group;
        }

        public async Task RemoveAsync(Location location)
        {
            _context.Locations.Remove(location);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Location location)
        {
            _context.Update(location);
            await _context.SaveChangesAsync();
        }
    }
}
