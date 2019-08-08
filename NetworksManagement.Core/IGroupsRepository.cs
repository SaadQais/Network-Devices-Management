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
        IQueryable<Group> GetAll();
        Task AddAsync(Group group, int[] locations);
        Task<Group> GetAsync(int? groupId);
        Task UpdateAsync(Group group, int[] locations);
        Task RemoveAsync(Group group);
        bool Any(int groupId);
        IEnumerable<Group> GetUserGroups(string userId);
    }
}
