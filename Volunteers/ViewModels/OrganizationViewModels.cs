using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Volunteers.ViewModels
{
    public class AddJob
    {
        public string Title { get; set; }
        [AllowHtml]
        public string Descriptions { get; set; }
        public HttpPostedFileBase Banner { get; set; }
        public string Location { get; set; }
        public string Start { get; set; }
        public string Finish { get; set; }
    }
}