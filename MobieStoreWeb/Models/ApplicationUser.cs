using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MobieStoreWeb.Models
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(450)]
        [Display(Name = "Name")]
        [PersonalData]
        public string Name { get; set; }

        [StringLength(2048)]
        [Display(Name = "Address")]
        [PersonalData]
        public string Address { get; set; }

        public virtual ICollection<ProductComment> ProductComments { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
