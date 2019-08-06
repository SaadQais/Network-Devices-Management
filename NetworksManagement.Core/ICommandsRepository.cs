using NetworksManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworksManagement.Core
{
    public interface ICommandsRepository
    {
        List<string> GetCmdList(Device device);
        string RunAutoUpdate();
        string GetDeviceVersion();
        string GetDeviceUptime();
        string AddDeviceUser(string name, string group, string password);
        List<string> GetBackupScript(Device device);
    }
}
