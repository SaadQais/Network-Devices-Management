﻿using NetworksManagement.Data.Enums;
using NetworksManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworksManagement.Data.ViewModels
{
    public class DeviceViewModel
    {
        public Device Device { get; set; }
        public IEnumerable<Group> Groups { get; set; }
        public IEnumerable<Location> Locations { get; set; }
        public IEnumerable<DeviceModel> Models { get; set; }
        public IEnumerable<DeviceAccount> Accounts { get; set; }
    }
}
