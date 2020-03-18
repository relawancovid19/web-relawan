﻿using System;
using System.Collections.Generic;
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
                var details = await repository.GetDetails(id);
                if (details != null)
                {
                    return View(details);
                }
            }
            return View("Error");
        }
    }
}