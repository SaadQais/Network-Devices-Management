using NetworksManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworksManagement.Core
{
    public interface IModelRepository
    {
        public IQueryable<DeviceModel> GetAll();
        public Task AddAsync(DeviceModel model);
        public Task<DeviceModel> GetAsync(int? modelId);
        public Task UpdateAsync(DeviceModel model);
        public Task RemoveAsync(DeviceModel model);
        public bool Any(int modelId);
    }
}
