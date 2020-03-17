using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Volunteers.Models
{
    public class Province
    {
        [Key]
        public string IdProvince { get; set; }
        public string Name { get; set; }
        public int Timezone { get; set; }
        public bool IsActive { get; set; }
    }
}