using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Volunteers.Controllers
{
    public class UploadImagesController : BaseController
    {
        // GET: UploadImages
        [HttpGet]
        public async Task<ActionResult> GetOrganizationtAvatar(string id)
        {
            if (id != null)
            {
                try
                {
                    var organization = await db.Organizations
                        .Where(x => x.Id == id).SingleOrDefaultAsync();
                    if (organization != null)
                    {
                        return PartialView("_GetOrganizationAvatar", organization);
                    }
                }
                catch (Exception ex)
                {
                    Trace.TraceError(ex.Message);
                    Trace.TraceError(ex.StackTrace);
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }
        [HttpPost]
        public async Task<ActionResult> UploadOrganizationAvatar(HttpPostedFileBase file, string id)
        {
            try
            {
                var organization = await db.Organizations.Where(x => x.Id == id).SingleOrDefaultAsync();
                if (organization != null)
                {
                    organization.Avatar = await Helpers.UploadFileHelper.UploadOrganizationImageAsync(file);
                    db.Entry(organization).State = EntityState.Modified;
                    var result = await db.SaveChangesAsync();
                    if (result > 0)
                        return new HttpStatusCodeResult(HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
            }
            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }
    }
}