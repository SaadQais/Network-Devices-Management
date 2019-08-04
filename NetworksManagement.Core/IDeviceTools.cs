using NetworksManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworksManagement.Core
{
    public interface IDeviceTools
    {
        public (bool, string) ExecuteSSHCommand(Device device, string script, string username, string password);
    }
}
