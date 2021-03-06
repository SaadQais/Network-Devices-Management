﻿using NetworksManagement.Core;
using NetworksManagement.Data.Models;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworksManagement.Infrastructure.Utils
{
    public class CisscoTools : IDeviceTools
    {
        public (bool, string) ExecuteSSHCommand(Device device, string script, string username, string password)
        {
            string exceptionMessage = "";

            script = script.Replace("\r\n", ";");

            foreach (var item in device.Interfaces)
            {
                try
                {

                    using SshClient ssh = new SshClient(item.Address, username ?? "", password ?? "");
                    {
                        ssh.Connect();

                        var response = ssh.RunCommand(script ?? "");

                        ssh.Disconnect();

                        return (true, response.Result);
                    }
                }
                catch (Exception ex)
                {
                    exceptionMessage = ex.Message;
                }
            }

            return (false, exceptionMessage);
        }

    }
}
