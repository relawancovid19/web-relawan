using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Volunteers.Models;

namespace Volunteers.Infrastructures
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobTransaction> JobTransactions { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Province> Provinces { get; set; }
    }
}