using Microsoft.EntityFrameworkCore;
using NetworksManagement.Core;
using NetworksManagement.Data;
using NetworksManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworksManagement.Infrastructure
{
    public class InterfacesRepository : IInterfacesRepository
    {
        private readonly ApplicationDbContext _context;
        public InterfacesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Interface> GetByGroupId(int? groupId)
        {
            var interfaces = _context.Interfaces.Include(i => i.Device).Where(i => i.Device.GroupId == groupId);

            return interfaces;
        }
    }
}
