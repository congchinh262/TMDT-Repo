using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MobieStoreWeb.Models
{
    public class Manufacturer
    {
        public short Id { get; set; }

        [StringLength(256,ErrorMessage ="{0} must be less than {1} character.")]
        [Required(ErrorMessage ="{0} is required.")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
