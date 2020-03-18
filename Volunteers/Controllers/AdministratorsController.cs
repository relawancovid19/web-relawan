using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Volunteers.Models;
using Volunteers.Repositories;

namespace Volunteers.Controllers
{
    [Authorize(Roles = "SA")]
    public class AdministratorsController : BaseController
    {
        private AdministratorRepository repository = new AdministratorRepository();
        // GET: Administrators
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public async Task<ActionResult> Jobs()
        {
            var jobs = await repository.GetJobs();
            return View(jobs);
        }
        public async Task<ActionResult> Organizations()
        {
            var organizations = await repository.GetOrganizations();
            return View(organizations);
        }
        public JsonResult IsEmailExsist(string email)
        {
            return Json(!db.Users.Any(x => x.Email == email)
                , JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> AddOrganization()
        {
            var provinces = await db.Provinces.Where(x => x.IsActive == true)
                .Select(i => new SelectListItem()
                {
                    Text = i.Name,
                    Value = i.IdProvince,
                    Selected = false
                }).ToArrayAsync();

            ViewBag.Provinces = provinces;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddOrganization(ViewModels.AddOrganization data)
        {
            if (ModelState.IsValid)
            {
                var currentUTCTime = DateTimeOffset.UtcNow;
                var province = await repository.GetProvince(data.Province);
                var addOrganization = new Models.Organization()
                {
                    Id = Guid.NewGuid().ToString(),
                    FullName = data.FullName,
                    Title = data.Title,
                    Registered = currentUTCTime,
                    Updated = currentUTCTime,
                    Institution = data.Institution,
                    Email = data.Email,
                    EmailConfirmed = true,
                    PhoneNumber = data.PhoneNumber,
                    UserName = data.Email,
                    RegistrationStatus = RegistrationStatus.Approved,
                    Address = data.Address,
                    FacebookUrl = data.FacebookUrl,
                    InstagramUrl = data.InstagramUrl,
                    TwitterUrl = data.TwitterUrl,
                    LinkedInUrl = data.LinkedInUrl,
                    YoutubeUrl = data.YoutubeUrl
                };
                try
                {
                    var addOrganizerResult = await UserManager.CreateAsync(addOrganization, data.Password);
                    var currentOrganization = await UserManager.FindByEmailAsync(data.Email);
                    var addToRoleResult = await UserManager.AddToRoleAsync(currentOrganization.Id, "Administrator");
                    if (addOrganizerResult.Succeeded && addToRoleResult.Succeeded)
                    {
                        var addOrganizationProvince = await repository.AddOrganizerProvince(addOrganization.Id, province);
                        if (addOrganizationProvince)
                        {
                            return RedirectToAction("Organizations");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Trace.TraceError(ex.Message);
                    Trace.TraceError(ex.StackTrace);
                }
            }
            var provinces = await db.Provinces.Where(x => x.IsActive == true)
                .Select(i => new SelectListItem()
                {
                    Text = i.Name,
                    Value = i.IdProvince,
                    Selected = false
                }).ToArrayAsync();

            ViewBag.Provinces = provinces;
            return View();
        }

        public async Task<ActionResult> Provinces()
        {
            var provinces = await repository.GetProvinces();
            return View(provinces);
        }
    }
}