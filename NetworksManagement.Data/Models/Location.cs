using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NetworksManagement.Data.Models
{
    public class Location
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<LocationsGroups> LocationsGroups { get; set; }
    }
}
