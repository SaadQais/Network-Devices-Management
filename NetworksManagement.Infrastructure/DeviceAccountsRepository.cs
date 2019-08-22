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
    public class DeviceAccountsRepository : IDeviceAccountsRepository
    {
        private readonly ApplicationDbContext _context;
        public DeviceAccountsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAccountAsync(DeviceAccount account)
        {
            if(account != null)
            {
                await _context.Accounts.AddAsync(account);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAccountAsync(int accountId)
        {
            var account = _context.Accounts.FirstOrDefault(a => a.Id == accountId);

            if(account != null)
            {
                _context.Accounts.Remove(account);
                await _context.SaveChangesAsync();
            }
        }

        public IEnumerable<DeviceAccount> GetDeviceAccounts(int deviceId)
        {
            var accounts = _context.Accounts.Where(a => a.DeviceId == deviceId);

            return accounts;
        }
    }
}
