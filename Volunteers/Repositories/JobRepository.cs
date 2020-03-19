using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Volunteers.Infrastructures;

namespace Volunteers.Repositories
{
    public class JobRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public async Task<Models.Job> GetDetails (string id)
        {
            var job = await db.Jobs.Include("Organization").Where(x => x.Id == id).SingleOrDefaultAsync();
            return job;
        }
    }
}