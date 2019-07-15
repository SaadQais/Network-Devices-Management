﻿using Microsoft.EntityFrameworkCore;
using NetworksManagement.Core;
using NetworksManagement.Data;
using NetworksManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworksManagement.Infrastructure
{
    public class GroupsRepository : IGroupsRepository
    {
        private readonly ApplicationDbContext _context;
        public GroupsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Group group)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Group> GetAll()
        {
            var groups = _context.Groups.Include(g => g.LocationsGroups).OrderBy(g => g.Name);

            return groups;
        }
    }
}
