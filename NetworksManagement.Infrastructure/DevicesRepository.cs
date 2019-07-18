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
    public class DevicesRepository : IDevicesRepository
    {
        private readonly ApplicationDbContext _context;
        public DevicesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Device device)
        {
            _context.Add(device);

            await _context.SaveChangesAsync();
        }

        public bool Any(int deviceId)
        {
            return _context.Devices.Any(l => l.Id == deviceId);
        }

        public IQueryable<Device> GetAll()
        {
            var devices = _context.Devices.Include(d => d.Group).OrderBy(g => g.Name);

            return devices;
        }

        public async Task<Device> GetAsync(int? deviceId)
        {
            var device = await _context.Devices.FirstOrDefaultAsync(l => l.Id == deviceId);

            return device;
        }

        public async Task RemoveAsync(Device device)
        {
            _context.Devices.Remove(device);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Device device)
        {
            _context.Update(device);
            await _context.SaveChangesAsync();
        }
    }
}
