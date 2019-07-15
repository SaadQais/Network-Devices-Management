using NetworksManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworksManagement.Data.ViewModels
{
    public class GroupViewModel
    {
        public Group Group { get; set; }
        public IEnumerable<Group> Groups { get; set; }
        public IEnumerable<Location> Locations { get; set; }
    }
}
