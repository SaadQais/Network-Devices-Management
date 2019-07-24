using Microsoft.EntityFrameworkCore;
using NetworksManagement.Core;
using NetworksManagement.Data;
using NetworksManagement.Data.Models;
using NetworksManagement.Infrastructure.Extensions;
using System;
using System.Linq;
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
            var group = await _context.Groups.Include(g => g.LocationsGroups).Include(g => g.Devices)
                .FirstOrDefaultAsync(g => g.Id == groupId);

            return group;
        }

        public IQueryable<Group> GetAll()
        {
            var groups = _context.Groups.Include(g => g.LocationsGroups).OrderBy(g => g.Name);

            return groups;
        }

        public async Task UpdateAsync(Group group, int[] locations)
        {
            var groupFromDb = await _context.Groups.Include(g => g.LocationsGroups).FirstOrDefaultAsync(g => g.Id == group.Id);
            var selectedLocations = await _context.Locations.Where(l => locations.Contains(l.Id)).ToListAsync();

            groupFromDb.IpRange = group.IpRange;
            groupFromDb.Name = group.Name;

            _context.TryUpdateManyToMany(groupFromDb.LocationsGroups, selectedLocations
                .Select(x => new LocationsGroups
                {
                     LocationId = x.Id,
                     GroupId = group.Id
                }), x => x.LocationId);
            
            await _context.SaveChangesAsync();
        }

        public bool Any(int groupId)
        {
            return _context.Groups.Any(g => g.Id == groupId);
        }

        public async Task RemoveAsync(Group group)
        {
            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();
        }
    }
}
