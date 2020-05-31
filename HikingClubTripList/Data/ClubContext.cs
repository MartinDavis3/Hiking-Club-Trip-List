using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HikingClubTripList.Models;

namespace HikingClubTripList.Data
{
    public class ClubContext : DbContext
    {
        public ClubContext(DbContextOptions<ClubContext> options) : base(options)
        {
        }

        public DbSet<Trip> Trips { get; set; }
        public DbSet<Signup> Signups { get; set; }
        public DbSet<Member> Members { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Trip>().ToTable("Trip");
            modelBuilder.Entity<Signup>().ToTable("Signup");
            modelBuilder.Entity<Member>().ToTable("Member");
        }
    }
}
