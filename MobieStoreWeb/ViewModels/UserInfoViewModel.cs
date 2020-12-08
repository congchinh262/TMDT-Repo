using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MobieStoreWeb.ViewModels
{
    public class UserInfoViewModel
    {
        [StringLength(450)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [StringLength(2048)]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [RegularExpression(@"^\d{10}$")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }
}
