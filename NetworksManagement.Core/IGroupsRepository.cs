using NetworksManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworksManagement.Core
{
    public interface IGroupsRepository
    {
        public IQueryable<Group> GetAll();
        public Task AddAsync(Group group, int[] locations);
        public Task<Group> GetAsync(int? groupId);
        public Task UpdateAsync(Group group, int[] locations);
        public Task RemoveAsync(Group group);
        public bool Any(int groupId);
    }
}
