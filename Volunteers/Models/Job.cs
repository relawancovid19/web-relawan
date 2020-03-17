using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Volunteers.Models
{
    [Table("Jobs")]
    public class Job
    {
        [Key]
        public string Id { get; set; }
        public string Title { get; set; }
        public string Descriptions { get; set; }
        public string Banner { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Start { get; set; }
        public DateTimeOffset Finish { get; set; }
        public Organization Organization { get; set; }
        public int Amount { get; set; }
    }
    [Table("JobTransactions")]
    public class JobTransaction
    {
        [Key]
        public string IdTransaction { get; set; }
        public Job Job { get; set; }
        public ApplicationUser Volunteer { get; set; }
        public DateTimeOffset Registered { get; set; }
        public RegistrationStatus Status { get; set; }
    }
}