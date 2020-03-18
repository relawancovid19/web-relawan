using System;
using System.Collections.Generic;
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
    }
}