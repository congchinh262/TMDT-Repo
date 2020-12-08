using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MobieStoreWeb.Models
{
    public class OrderDetail
    {
        public int OrderId { get; set; }

        public virtual Order Order { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }


        [Required(ErrorMessage = "{0} is required.")]
        [Display(Name = "Price")]
        [Column(TypeName = "decimal(18,4)")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "The {0} must be at least {1} at max {2}.")]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }        

    }
}
