using NetworksManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworksManagement.Core
{
    public interface ILocationsRepository
    {
        public IQueryable<Location> GetAll();
        public void Add(Location location);
    }
}
