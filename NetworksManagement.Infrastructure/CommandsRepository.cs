﻿using NetworksManagement.Core;
using NetworksManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworksManagement.Infrastructure
{
    public class CommandsRepository : ICommandsRepository
    {
        public List<string> GetCmdList(Device device)
        {
            List<string> cmdList = new List<string>();

            foreach(var item in device.Interfaces)
            {
                cmdList.Add(AddIpAddress(item.Address, item.Name));
            }

            cmdList.Add(SetIdentity(device.Name));

            return cmdList;
        }

        public string RunAutoUpdate()
        {
            return $"system package update install ;";
        }

        public string GetDeviceVersion()
        {
            return $" :put [system resource get version] ;";
        }

        public string GetDeviceUptime()
        {
            return $" :put [system resource get uptime] ;";
        }

        public List<string> GetBackupScript(Device device)
        {
            List<string> cmdList = new List<string>();

            string script = @"{:local FTPServer ""10.1.90.224""
                        :local FTPPort 21 
                        :local FTPUser ""Administrator""
                        :local FTPPass ""SoftwareR2"" 
                        :local fname ([/system identity get name]) 
                        :local sfname (""/"".$fname) 
                        /export file=($sfname) 
                        :local backupFileName """" 
                        :foreach backupFile in=[/file find] do={ 
                            :set backupFileName (""/"".[/file get $backupFile name])
                            :if ([:typeof [:find $backupFileName $sfname]] != ""nil"") do={ 
                                /tool fetch address=$FTPServer port=$FTPPort src-path=$backupFileName user=$FTPUser mode=ftp password=$FTPPass dst-path=$backupFileName upload=yes 
                        } 
                        }
                        :delay 5s;
                        :foreach i in=[/file find] do={:if ([:typeof [:find [/file get $i name] [/system identity get name]]]!=""nil"") do={/file remove $i}}
                        :log info message=""Successfully removed Temporary Backup Files""
                        :log info message=""Automatic Backup Completed Successfully""
                        ;}";

            string backupName = $"backup";
            
            cmdList.Add($"system script add name={backupName} source= { script }" );
            cmdList.Add($"system script run number={backupName} ;");
            cmdList.Add($"system script remove number={backupName} ;");

            return cmdList;
        }

        public string AddDeviceUser(string name, string group, string password)
        {
            return $"user add name={name} group={group} password={password}";
        }

        private string SetIdentity(string identity)
        {
            return $"system identity set name= { identity } ;";
        }

        private string AddIpAddress(string ip, string ether)
        {
            return $"ip address add address= { ip } interface= { ether } ;";
        }

    }
}
