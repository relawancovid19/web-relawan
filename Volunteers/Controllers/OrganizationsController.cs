using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Volunteers.Repositories;

namespace Volunteers.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class OrganizationsController : BaseController
    {
        private OrganizationRepository repository = new OrganizationRepository();
        // GET: Organizations
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public async Task<ActionResult> Jobs()
        {
            var jobs = await repository.GetJobs(User.Identity.Name);
            return View(jobs);
        }

        public async Task<ActionResult> AddJob()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> AddJob(ViewModels.AddJob data)
        {
            if (ModelState.IsValid)
            {
                var organization = await repository.GetOrganization(User.Identity.Name);
                if(organization != null)
                {
                    var addJob = await repository.AddJob(data, organization);
                    if (addJob)
                    {
                        return RedirectToAction("Jobs");
                    }
                }
            }
            return View();
        }
        public async Task<ActionResult> EditJob(string id)
        {
            var job = await repository.GetJob(id);
            return View(job);
        }
        [HttpPost]
        public async Task<ActionResult> EditJob(ViewModels.AddJob data)
        {
            var job = await db.Jobs.Where(x=>x.Id == data.Id).SingleOrDefaultAsync();
            if(job != null)
            {
                DateTimeFormatInfo fmt = new CultureInfo("id-id").DateTimeFormat;
                DateTimeOffset start = job.Start;
                DateTimeOffset finish = job.Finish;

                if(data.Start != null && data.Finish != null)
                {
                    start = DateTimeOffset.Parse(data.Start ?? job.Start.ToString(), fmt);
                    finish = DateTimeOffset.Parse(data.Finish ?? job.Finish.ToString(), fmt);
                }
                

                job.Title = data.Title ?? job.Title;
                job.Location = data.Location ?? job.Location ;
                job.Start = start;
                job.Finish = finish;
                job.Descriptions = data.Descriptions ?? job.Descriptions;
                job.Banner = await Helpers.UploadFileHelper.UploadBannerImage(data.Banner) ?? job.Banner;
            };
            try
            {
                db.Entry(job).State = EntityState.Modified;
                var result = await db.SaveChangesAsync();
                if (result > 0)
                {
                    return RedirectToAction("Jobs");
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
            }
            return View(job);
        }

        public async Task<ActionResult> Volunteers()
        {
            var volunteers = await db.JobTransactions.Include("Volunteer").Where(x => x.Job.Organization.UserName == User.Identity.Name).ToListAsync();
            return View(volunteers);
        }

        public async Task<ActionResult> VolunteerJobs(string id)
        {
            var volunteers = await db.JobTransactions.Include("Volunteer").Where(x => x.Job.Id == id).ToListAsync();
            return View(volunteers);
        }
    }
}