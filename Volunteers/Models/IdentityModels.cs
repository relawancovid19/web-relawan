using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Volunteers.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string Institution { get; set; }
        public string Title { get; set; }
        public string FacebookUrl { get; set; }
        public string InstagramUrl { get; set; }
        public string TwitterUrl { get; set; }
        public string LinkedInUrl { get; set; }
        public string YoutubeUrl { get; set; }
        public string WebsiteUrl { get; set; }
        public string Avatar { get; set; }
        public string Address { get; set; }
        public DateTimeOffset Registered { get; set; }
        public DateTimeOffset Updated { get; set; }
        public bool IsBanned { get; set; }
        public virtual Province Province { get; set; }
        
        public RegistrationStatus RegistrationStatus { get; set; }
        public InvitationStatus InvitationStatus { get; set; }
        public ApplicationUser InvitedBy { get; set; }
        public string Descriptions { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
    public enum RegistrationStatus
    {
        Rejected,
        Approved,
        Pending
    }
    public enum InvitationStatus
    {
        Rejected,
        Approved,
        Pending
    }
}