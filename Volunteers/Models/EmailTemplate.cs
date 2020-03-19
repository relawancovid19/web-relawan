using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Volunteers.Models
{
    [Table("EmailTemplates")]
    public class EmailTemplate
    {
        [Key]
        public string IdEmailTemplate { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }
}