using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NetworksManagement.Data.Models
{
    public class DeviceAccount
    {
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public Permissions Permission { get; set; }

        public int DeviceId { get; set; }
        [ForeignKey("DeviceId")]
        public Device Device { get; set; }
    }
}
