using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Volunteers.Infrastructures;

namespace Volunteers.Repositories
{
    public class AdministratorRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public async Task<IEnumerable<Models.Job>> GetJobs()
        {
            return await db.Jobs.ToListAsync();
        }
        public async Task<IEnumerable<Models.Organization>> GetOrganizations()
        {
            return await db.Organizations.ToListAsync();
        }
        public async Task<IEnumerable<Models.Province>> GetProvinces()
        {
            return await db.Provinces.ToListAsync();
        }
        public async Task<Models.Province> GetProvince(string id)
        {
            return await db.Provinces.Where(x => x.IdProvince == id).SingleOrDefaultAsync();
        }
        public async Task<bool> AddOrganizerProvince(string id, Models.Province province)
        {
            var addOrganizationProvince = await db.Organizations.Include("Province").
                            Where(x => x.Id == id).SingleOrDefaultAsync();
            addOrganizationProvince.Province = province;
            var result = await db.SaveChangesAsync();
            if (result > 0)
            {
                return true;
            }
            return false;
        }
    }
}