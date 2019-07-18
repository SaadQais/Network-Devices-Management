using NetworksManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworksManagement.Core
{
    public interface IInterfacesRepository
    {
        public IEnumerable<Interface> GetByGroupId(int? groupId);
    }
}
