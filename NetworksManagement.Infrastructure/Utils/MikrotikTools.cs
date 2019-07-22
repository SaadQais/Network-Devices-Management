using NetworksManagement.Data.Models;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworksManagement.Infrastructure.Utils
{
    public class MikrotikTools
    {
        public string ExecuteSSHCommand(Device device, string script)
        {
            try
            {
                script = script.Replace("\r\n", ";");

                using SshClient ssh = new SshClient(device.Interfaces.First().Address, "admin", "");
                    ssh.Connect();

                var response = ssh.RunCommand(script);

                ssh.Disconnect();

                return response.Result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
