using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NetworksManagement.Data.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public String Name { get; set; }
    }
}
