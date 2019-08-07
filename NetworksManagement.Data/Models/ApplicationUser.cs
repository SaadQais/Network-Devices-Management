using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NetworksManagement.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }

        [NotMapped]
        public bool SuperAdmin { get; set; }

        public virtual ICollection<ApplicationUserGroups> ApplicationUserGroups { get; set; }
    }
}
