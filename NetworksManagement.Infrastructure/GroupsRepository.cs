using Microsoft.EntityFrameworkCore;
using NetworksManagement.Core;
using NetworksManagement.Data;
using NetworksManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworksManagement.Infrastructure
{
    public class GroupsRepository : IGroupsRepository
    {
        private readonly ApplicationDbContext _context;
        public GroupsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Group group, int[] locations)
        {
            _context.Add(group);

            await _context.SaveChangesAsync();

            foreach (var locationId in locations)
            {
                _context.LocationsGroups.Add(new LocationsGroups
                {
                    LocationId = locationId,
                    GroupId = group.Id
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task<Group> GetAsync(int? groupId)
        {
            var group = await _context.Groups.Include(g => g.LocationsGroups).FirstOrDefaultAsync(g => g.Id == groupId);

            return group;
        }

        public IQueryable<Group> GetAll()
        {
            var groups = _context.Groups.Include(g => g.LocationsGroups).OrderBy(g => g.Name);

            return groups;
        }


    }
}
