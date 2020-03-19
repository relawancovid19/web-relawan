using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Volunteers.Repositories;

namespace Volunteers.Controllers
{
    public class JobsController : BaseController
    {
        private JobRepository repository = new JobRepository();
        // GET: Jobs
        public async Task<ActionResult> Details(string id)
        {
            if (id != null)
            {
                var province = await db.Provinces
                         .Select(i => new SelectListItem()
                         {
                             Text = i.Name,
                             Value = i.IdProvince,
                             Selected = false
                         }).ToArrayAsync();
                ViewBag.Provinces = province;
                var transaction = await db.JobTransactions.Where(x => x.Volunteer.UserName == User.Identity.Name && x.Job.Id == id).OrderByDescending(x => x.Registered).FirstOrDefaultAsync();
                if(transaction != null)
                {
                    ViewBag.Status = transaction.Status.ToString();
                }

                var details = await repository.GetDetails(id);
                if (details != null)
                {
                    return View(details);
                }
            }
            return View("Error");
        }
        [HttpPost]
        public async Task<ActionResult> Apply(ViewModels.ApplyJobs data)
        {
            if (ModelState.IsValid)
            {
                var user = await db.Users.Where(x => x.Email == data.Email).SingleOrDefaultAsync();
                if (user == null)
                {
                    var currentUTCTime = DateTimeOffset.UtcNow;
                    var province = await db.Provinces.Where(x => x.IdProvince == data.Province).SingleOrDefaultAsync();
                    var addOrganization = new Models.ApplicationUser()
                    {
                        Id = Guid.NewGuid().ToString(),
                        FullName = data.Name,
                        Title = data.Title,
                        Registered = currentUTCTime,
                        Updated = currentUTCTime,
                        Institution = data.Institution,
                        Email = data.Email,
                        EmailConfirmed = true,
                        PhoneNumber = data.PhoneNumber,
                        UserName = data.Email,
                        Descriptions = data.Message
                    };
                    try
                    {
                        var addVolunteer = await UserManager.CreateAsync(addOrganization, data.Password);
                        var currentVolunteer = await UserManager.FindByEmailAsync(data.Email);
                        var addToRoleResult = await UserManager.AddToRoleAsync(currentVolunteer.Id, "Volunteer");
                        if (addVolunteer.Succeeded && addToRoleResult.Succeeded)
                        {
                            await SignInManager.SignInAsync(addOrganization, isPersistent: false, rememberBrowser: false);
                            var addOrganizationProvince = await db.Users.Include("Province").
                                                        Where(x => x.Id == currentVolunteer.Id).SingleOrDefaultAsync();
                            addOrganizationProvince.Province = province;
                            var result = await db.SaveChangesAsync();
                            if (result > 0)
                            {
                                var addTransaction = await ApplyJob(currentVolunteer.UserName, data.Id);
                                return Json("OK", JsonRequestBehavior.AllowGet);
                            }
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
                    return Json("REGISTERED", JsonRequestBehavior.AllowGet);
                }
            }
            return View();
        }
        public async Task<ActionResult> ApplyJob(string username, string id)
        {
            if (User.Identity.IsAuthenticated)
            {
                username = User.Identity.Name;
            }
            var jobs = await db.Jobs.Include("Organization").Where(x => x.Id == id).SingleOrDefaultAsync();
            if (jobs != null)
            {
                var volunteer = await db.Users.Where(x => x.UserName == username).SingleOrDefaultAsync();
                var transaction = await db.JobTransactions.Where(x => x.Volunteer.UserName == User.Identity.Name && x.Job.Id == id).OrderByDescending(x => x.Registered).FirstOrDefaultAsync();
                if (transaction == null)
                {
                    var newTransaction = new Models.JobTransaction()
                    {
                        IdTransaction = Guid.NewGuid().ToString(),
                        Registered = DateTimeOffset.UtcNow,
                        Job = jobs,
                        Volunteer = volunteer,
                        Status = Models.RegistrationStatus.Pending
                    };
                    db.JobTransactions.Add(newTransaction);
                    var result = await db.SaveChangesAsync();
                    if (result > 0)
                    {
                        await SendJobRegistrationEmail(newTransaction.Job, volunteer);
                        await SendJobOrganizerRegistrationEmail(newTransaction.Job, volunteer);
                        return Json("OK", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json("EXIST", JsonRequestBehavior.AllowGet);
                }
            }
            return Json("NOTLOGIN", JsonRequestBehavior.AllowGet);
        }

        private async Task SendJobRegistrationEmail(Models.Job job, Models.ApplicationUser volunteer)
        {
            //1. Dapatkan email template yang dipakai untuk mengirim proses registrasi 
            var registrationEmail = await db.EmailTemplates.FindAsync("job-registration-volunteer");
            if (registrationEmail != null)
            {
                var emailBody = registrationEmail.Content.Replace("[FullName]", volunteer.FullName)
                                .Replace("[Title]", job.Title)
                                .Replace("[Day]", Helpers.DateTimeHelper.GetLocalDay(job.Start))
                                .Replace("[Date]", Helpers.DateTimeHelper.GetLocalDate(job.Start))
                                .Replace("[Time]", Helpers.DateTimeHelper.GetLocalTime(job.Start))
                                .Replace("[Location]", job.Location)
                                .Replace("[Organizer]", job.Organization.FullName);
                await Helpers.EmailHelper.Send(registrationEmail.Subject.Replace("[Title]", job.Title), volunteer.Email, volunteer.FullName, emailBody);
            }
        }

        private async Task SendJobOrganizerRegistrationEmail(Models.Job job, Models.ApplicationUser volunteer)
        {
            //1. Dapatkan email template yang dipakai untuk mengirim proses registrasi 
            var registrationEmail = await db.EmailTemplates.FindAsync("send-job-registration");
            if (registrationEmail != null)
            {
                var emailBody = registrationEmail.Content.Replace("[FullName]", volunteer.FullName)
                                .Replace("[Title]", job.Title)
                                .Replace("[Day]", Helpers.DateTimeHelper.GetLocalDay(job.Start))
                                .Replace("[Date]", Helpers.DateTimeHelper.GetLocalDate(job.Start))
                                .Replace("[Time]", Helpers.DateTimeHelper.GetLocalTime(job.Start))
                                .Replace("[Location]", job.Location)
                                .Replace("[Organizer]", job.Organization.FullName);
                await Helpers.EmailHelper.Send(registrationEmail.Subject.Replace("[Title]", job.Title), job.Organization.Email, volunteer.FullName, emailBody);
            }
        }

        public JsonResult IsEmailSpeakerExists(string email)
        {
            return Json(!db.Users.Any(x => x.Email == email)
                , JsonRequestBehavior.AllowGet);
        }
    }
}