using NetworksManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworksManagement.Core
{
    public interface IGroupsRepository
    {
        public IEnumerable<Group> GetAll();
        public void Add(Group group);
    }
}
