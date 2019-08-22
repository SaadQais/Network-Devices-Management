using NetworksManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetworksManagement.Core
{
    public interface IDeviceAccountsRepository
    {
        IEnumerable<DeviceAccount> GetDeviceAccounts(int deviceId);
        Task AddAccountAsync(DeviceAccount account);
        Task DeleteAccountAsync(int accountId);
    }
}
