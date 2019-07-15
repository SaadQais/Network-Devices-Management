using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetworksManagement.Data.Models;

namespace NetworksManagement.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(local);Database=NetworksManagementDb;Trusted_Connection=True;Persist Security Info=True;MultipleActiveResultSets=true");
            }
        }

        public DbSet<Location> Locations { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<LocationsGroups> LocationsGroups { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}
