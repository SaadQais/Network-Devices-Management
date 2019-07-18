using NetworksManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworksManagement.Core
{
    public interface IDevicesRepository
    {
        public IQueryable<Device> GetAll();
        public Task AddAsync(Device device);
        public Task<Device> GetAsync(int? deviceId);
        public Task UpdateAsync(Device device, List<Interface> interfaces);
        public Task RemoveAsync(Device device);
        public bool Any(int deviceId);
    }
}
