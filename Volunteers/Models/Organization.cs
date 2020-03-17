using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Volunteers.Models
{
    [Table("Organizations")]
    public class Organization : ApplicationUser
    {
        public ApplicationUser CreatedBy { get; set; }
    }
}