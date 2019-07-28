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
    public class ModelRepository : IModelRepository
    {
        private readonly ApplicationDbContext _context;
        public ModelRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(DeviceModel model)
        {
            _context.Add(model);
            await _context.SaveChangesAsync();
        }

        public bool Any(int modelId)
        {
            return _context.DeviceModels.Any(m => m.Id == modelId);
        }

        public IQueryable<DeviceModel> GetAll()
        {
            var models = _context.DeviceModels.Include(m => m.Category).OrderBy(g => g.Name);
            return models;
        }

        public async Task<DeviceModel> GetAsync(int? modelId)
        {
            var model = await _context.DeviceModels.Include(m => m.Category).FirstOrDefaultAsync(g => g.Id == modelId);
            return model;
        }

        public async Task RemoveAsync(DeviceModel model)
        {
            _context.DeviceModels.Remove(model);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DeviceModel model)
        {
            _context.Update(model);
            await _context.SaveChangesAsync();
        }
    }
}
