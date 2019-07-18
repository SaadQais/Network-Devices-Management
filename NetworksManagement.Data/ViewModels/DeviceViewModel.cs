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
    }
}
