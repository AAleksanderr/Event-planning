using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using EventPlanning.Models;

namespace EventPlanning.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<EventModel> EventModels { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
                        .Property(e => e.NickName)
                        .HasMaxLength(50);

            modelBuilder.Entity<ApplicationUser>()
                        .Property(e => e.City)
                        .HasMaxLength(20);

            modelBuilder.Entity<ApplicationUser>()
                        .Property(e => e.IsAdmin);
        }
    }
}
