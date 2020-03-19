using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Volunteers.ViewModels
{
    public class JobsViewModels
    {
    }
    public class ApplyJobs
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string PhoneNumber { get; set; }
        public string Title { get; set; }
        public string Institution { get; set; }
        public string Province { get; set; }
        public string Message { get; set; }
        public string Id { get; set; }
    }
}