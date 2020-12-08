using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MobieStoreWeb.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        [Display(Name = "Category")]
        public short CategoryId { get; set; }

        public virtual Category Category { get; set; }


        [Display(Name = "Manufacturer")]
        public short ManufacturerId { get; set; }

        public virtual Manufacturer Manufacturer { get; set; }

        [StringLength(256, ErrorMessage = "{0} must be less than {1} character.")]
        [Required(ErrorMessage = "{0} is required.")]
        [Display(Name = "Name")]
        public string Name { get; set; }


        [Range(0, double.PositiveInfinity, ErrorMessage = "{0} must be at least {1}.")]
        [Required(ErrorMessage = "{0} is required.")]
        [Display(Name = "Price")]
        [Column(TypeName = "decimal(18,4)")]
        public decimal Price { get; set; }

        [Display(Name= "Quantity")]
        [Required(ErrorMessage = "{0} is required.")]
        [Range(0,int.MaxValue,ErrorMessage ="The {0} must be at least {1} at max {2}.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        public string Image { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }


        [Required(ErrorMessage = "{0} is required.")]
        [Column(TypeName = "nvarchar(24)")]
        [Display(Name = "Status")]
        public ProductStatus Status { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        [Display(Name = "Publish Date")]
        public DateTime PublishDate { get; set; }

        public virtual ICollection<ProductComment> ProductComments { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        

    }

    public enum ProductStatus:byte {
        OutOfStock,
        Available,
        CommingSoon,
    }
}
