using NetworksManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworksManagement.Data.ViewModels
{
    public class UserViewModel
    {
        public ApplicationUser ApplicationUser { get; set; }
        public List<Group> Groups { get; set; }
        public List<ApplicationUserGroups> SelectedGroups { get; set; }
    }
}
