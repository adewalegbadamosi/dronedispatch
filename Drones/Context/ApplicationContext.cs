using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Drones.Models;
using Microsoft.EntityFrameworkCore;

namespace Drones.Context
{
    public class ApplicationContext : DbContext
    {
       
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public DbSet<Drone> Drones { get; set; }

        public DbSet<Medication> Medications { get; set; }

        public DbSet<Audit> Audits { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
