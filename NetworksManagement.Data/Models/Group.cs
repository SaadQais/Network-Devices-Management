using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NetworksManagement.Data.Models
{
    public class Group
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string IpRange { get; set; }

        public virtual ICollection<LocationsGroups> LocationsGroups { get; set; }
        public virtual ICollection<Device> Devices { get; set; }
        public virtual ICollection<ApplicationUserGroups> ApplicationUserGroups { get; set; }
    }
}
