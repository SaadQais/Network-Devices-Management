﻿using System;
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
        public DbSet<ApplicationUserGroups> ApplicationUserGroups { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Interface> Interfaces { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<DeviceModel> DeviceModels { get; set; }
        public DbSet<DeviceAccount> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LocationsGroups>(entity =>
            {
                entity.HasKey(e => new { e.LocationId, e.GroupId });

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.LocationsGroups)
                    .HasForeignKey(d => d.LocationId);

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.LocationsGroups)
                    .HasForeignKey(d => d.GroupId);
            });

            modelBuilder.Entity<ApplicationUserGroups>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.GroupId });

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ApplicationUserGroups)
                    .HasForeignKey(d => d.UserId);

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.ApplicationUserGroups)
                    .HasForeignKey(d => d.GroupId);
            });
        }
    }
}
