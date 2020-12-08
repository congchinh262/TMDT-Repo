using MobieStoreWeb.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MobieStoreWeb.ViewModels
{
    public class CheckoutViewModel
    {
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

        public bool IsGuest { get; set; }

        public CartViewModel Cart{ get; set; }
    }
}
