using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
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

        public async Task<ActionResult> DetailVolunteer(string id)
        {
            var volunteer = await db.JobTransactions.Include("Volunteer").Include("Job").Include("Volunteer.Province").Include("Job.Organization").Where(x => x.IdTransaction == id).SingleOrDefaultAsync();
            if (volunteer != null)
            {
                return View(volunteer);
            }
            return View("Error");
        }
        [HttpPost]
        public async Task<ActionResult> DetailVolunteer(ViewModels.UpdateTransaction data)
        {
            var volunteer = await db.JobTransactions.Include("Job").Include("Volunteer").Where(x => x.IdTransaction == data.IdTransaction).SingleOrDefaultAsync();
            if (volunteer != null)
            {
                volunteer.Status = data.Status;
                db.Entry(volunteer).State = EntityState.Modified;
                var result = await db.SaveChangesAsync();
                if(result > 0)
                {
                    if(data.Status == Models.RegistrationStatus.Approved)
                    {
                        await SendProgramRegistrationEmail(volunteer.Job, volunteer.Volunteer);
                    }
                    return RedirectToAction("VolunteerJobs", new { id = volunteer.Job.Id });
                }
            }
            return View("Error");
        }
        private async Task SendProgramRegistrationEmail(Models.Job job, Models.ApplicationUser volunteer)
        {
            //1. Dapatkan email template yang dipakai untuk mengirim proses registrasi 
            var registrationEmail = await db.EmailTemplates.FindAsync("program-registration");
            if (registrationEmail != null)
            {
                //contoh apabila ingin mengenerate sebuah URL dari sistem 
                //var profileUrl = Url.Action("ParticipantProfile", "Reservations", new { reservation = trackParticipant.IdTransaction }, protocol: Request.Url.Scheme);
                //var eventStatusURL = Url.Action("RegistrationsStatus", "Programs", new { id = participant.QRCode }, protocol: Request.Url.Scheme);
                //2. Replace di email template, sesuai dengan tag yang diperlukan, sebagai contoh [FullName] di replace dengan FullName dari participant 
                var emailBody = registrationEmail.Content.Replace("[FullName]", volunteer.FullName)
                                .Replace("[SessionName]", job.Title)
                                .Replace("[Date]", job.Start.ToString("dd-MM-yyyy"))
                                .Replace("[Start]", job.Start.ToString("HH:mm"))
                                .Replace("[End]", job.Finish.ToString("HH:mm"));
                //3. Kirim email dengan email service dari identity config 
                await Helpers.EmailHelper.Send(registrationEmail.Subject.Replace("[SessionName]", job.Title),volunteer.Email,volunteer.FullName, emailBody);
            }
        }
        [HttpGet]
        public async Task<ActionResult> EditProfile()
        {
            var username = User.Identity.Name;

            var organization = db.Organizations.Include("Province").FirstOrDefault(u => u.UserName.Equals(username));

            if (organization != null)
            {
                var provinces = await db.Provinces.Where(x => x.IsActive == true)
                       .Select(i => new SelectListItem()
                       {
                           Text = i.Name,
                           Value = i.IdProvince,
                           Selected = false
                       }).ToArrayAsync();
                if (organization.Province != null)
                {
                    foreach (var p in provinces)
                    {
                        if (organization.Province.IdProvince == p.Value)
                        {
                            p.Selected = true;
                        }
                    }
                }
                ViewBag.Provinces = provinces;
                return View(organization);
            }
            return View("Error");
        }
        [HttpPost]
        public async Task<ActionResult> EditProfile(ViewModels.EditOrganization data)
        {
            if (ModelState.IsValid)
            {
                var currentUTCTime = DateTimeOffset.UtcNow;
                var username = User.Identity.Name;

                var organization = db.Organizations.FirstOrDefault(u => u.UserName.Equals(username));
                var province = await db.Provinces.FindAsync(data.Province);
                if (organization != null && province != null)
                {
                    organization.FullName = data.FullName;
                    organization.Title = data.Title;
                    organization.Updated = currentUTCTime;
                    organization.Institution = data.Institution;
                    organization.PhoneNumber = data.PhoneNumber;
                    organization.Address = data.Address;
                    organization.Province = province;
                    organization.FacebookUrl = data.FacebookUrl;
                    organization.InstagramUrl = data.InstagramUrl;
                    organization.TwitterUrl = data.TwitterUrl;
                    organization.LinkedInUrl = data.LinkedInUrl;
                    organization.YoutubeUrl = data.YoutubeUrl;

                    try
                    {
                        db.Entry(organization).State = EntityState.Modified;
                        var result = await db.SaveChangesAsync();
                        if (result > 0)
                        {
                            return Json("OK", JsonRequestBehavior.AllowGet);

                        }
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceError(ex.Message);
                        Trace.TraceError(ex.StackTrace);
                    }
                }
                else
                {
                    return Json("KO", JsonRequestBehavior.AllowGet);
                }
            }
            return Json("KO", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> Profile()
        {
            var organization = await db.Organizations.Include("Province").Where(x => x.UserName == User.Identity.Name).SingleOrDefaultAsync();
            return View(organization);
        }
        
    }
}