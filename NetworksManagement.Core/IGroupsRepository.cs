using NetworksManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworksManagement.Core
{
    public interface IGroupsRepository
    {
        public IQueryable<Group> GetAll();
        public void Add(Group group);
    }
}
