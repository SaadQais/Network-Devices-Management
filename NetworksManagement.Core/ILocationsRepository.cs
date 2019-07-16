using NetworksManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworksManagement.Core
{
    public interface ILocationsRepository
    {
        public IQueryable<Location> GetAll();
        public Task AddAsync(Location location);
        public Task<Location> GetAsync(int? locationId);
        public Task UpdateAsync(Location location);
        public Task RemoveAsync(Location location);
        public bool Any(int locationId);
    }
}
