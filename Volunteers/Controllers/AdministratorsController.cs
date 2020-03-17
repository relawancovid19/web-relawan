using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Volunteers.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdministratorsController : Controller
    {
        // GET: Administrators
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public async Task<ActionResult> Jobs()
        {
            return View();
        }
    }
}