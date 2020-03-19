using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Volunteers.Infrastructures;

namespace Volunteers.Repositories
{
    public class OrganizationRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public async Task<IEnumerable<Models.Job>> GetJobs(string username)
        {
            return await db.Jobs.Include("Organization").Where(x=>x.Organization.UserName == username).ToListAsync();
        }
        public async Task<Models.Job> GetJob(string id)
        {
            return await db.Jobs.Include("Organization").Where(x => x.Id == id).SingleOrDefaultAsync();
        }
        public async Task<Models.Organization> GetOrganization(string username)
        {
            return await db.Organizations.Where(x => x.UserName == username).SingleOrDefaultAsync();
        }
        public async Task<bool> AddJob(ViewModels.AddJob data, Models.Organization organization)
        {
            DateTimeFormatInfo fmt = new CultureInfo("id-id").DateTimeFormat;
            DateTimeOffset start = DateTimeOffset.Parse(data.Start, fmt);
            DateTimeOffset finish = DateTimeOffset.Parse(data.Finish, fmt);

            var newJob = new Models.Job()
            {
                Id = data.Id,
                Start = start,
                Finish = finish,
                Title = data.Title,
                Location = data.Location,
                Created = DateTimeOffset.Now,
                Descriptions = data.Descriptions,
                Organization = organization,
                Banner = await Helpers.UploadFileHelper.UploadBannerImage(data.Banner)
            };
            try
            {
                db.Jobs.Add(newJob);
                var result = await db.SaveChangesAsync();
                if(result > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
            }
            return false;
        }
    }
}