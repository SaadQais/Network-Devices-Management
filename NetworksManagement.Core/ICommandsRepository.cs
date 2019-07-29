using NetworksManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworksManagement.Core
{
    public interface ICommandsRepository
    {
        public List<string> GetCmdList(Device device);
        public string GetAutoUpdate();
        public List<string> GetBackupScript(Device device);
    }
}
