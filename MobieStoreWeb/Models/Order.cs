using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MobieStoreWeb.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Display(Name = "User")]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        #region Shipping
        [Required(ErrorMessage = "{0} is required.")]
        [Display(Name = "Shipping Name")]
        public string ShippingName { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        [Display(Name = "Shipping Address")]
        public string ShippingAddress { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        [Display(Name = "Shipping Phone Number")]
        [RegularExpression(@"^\d{10}$")]
        public string ShippingPhoneNumber { get; set; }

        [Display(Name = "Delivery Option")]
        public DeliveryOption DeliveryOption { get; set; }
        #endregion

        #region Billing
        [Required(ErrorMessage = "{0} is required.")]
        [Display(Name = "Billing Name")]
        public string BillingName { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        [Display(Name = "Billing Address")]
        public string BillingAddress { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        [Display(Name = "Billing Phone Number")]
        [RegularExpression(@"^\d{10}$")]
        public string BillingPhoneNumber { get; set; }

        [Display(Name = "Payment Method")]
        public PaymentMethod PaymentMethod { get; set; }

        [Display(Name = "Payment Status")]
        public PaymentStatus PaymentStatus { get; set; }
        #endregion

        [Column(TypeName = "nvarchar(24)")]
        [Required(ErrorMessage = "{0} is required.")]
        [Display(Name = "Status")]
        public OrderStatus Status { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }


    }
    public enum OrderStatus
    {
        Pending,
        Processing,
        Completed,
        Cancelled
    }

    public enum PaymentStatus
    {
        Pending,
        Unpaid,
        Paid,
        Refunded,
        Voided,
    }

    public enum PaymentMethod
    {
        COD,
        Paypal,
    }

    public enum DeliveryOption
    {
        Normal,
        Express,
    }
}
