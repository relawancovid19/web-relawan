namespace Volunteers.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Volunteers.Infrastructures;
    using Volunteers.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<Volunteers.Infrastructures.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Volunteers.Infrastructures.ApplicationDbContext";
        }

        protected override void Seed(Volunteers.Infrastructures.ApplicationDbContext context)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

            if (roleManager.Roles.Count() == 0)
            {
                roleManager.Create(new IdentityRole { Name = "SA" });
                roleManager.Create(new IdentityRole { Name = "Volunteer" });
                roleManager.Create(new IdentityRole { Name = "Administrator" });
            };
            var admin = new ApplicationUser
            {
                PhoneNumber = "+62818271214",
                PhoneNumberConfirmed = true,
                UserName = "alex@cloudcomputing.id",
                Email = "alex@cloudcomputing.id",
                FullName = "Alex Budiyanto",
                Institution = "ACCI",
                Title = "CEO"
            };
            if (manager.FindByName("alex@cloudcomputing.id") == null)
            {
                manager.Create(admin, "Volunteer@2020");
                manager.AddToRoles(admin.Id, new string[] { "Administrator", "SA" });
            }
        }
    }
}
