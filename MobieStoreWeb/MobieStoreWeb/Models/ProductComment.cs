using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace MobieStoreWeb.Models
{
    public class ProductComment
    {
        public int Id { get; set; }

        [Display(Name = "Product")]
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        [Display(Name = "User")]
        public string UserId { get; set; }

        public virtual ApplicationUser User  { get; set; }

        [Display(Name = "Content")]
        public string Content { get; set; }
        
        [Range(0,10)]
        [Display(Name = "Rating")]
        public byte Rating { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}