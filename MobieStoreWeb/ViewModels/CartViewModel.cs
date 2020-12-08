using MobieStoreWeb.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MobieStoreWeb.ViewModels
{
    public class CartViewModel
    {
        public List<CartItemViewModel> Items { get; set; } = new List<CartItemViewModel>();

        [JsonIgnore]
        public decimal Total
        {
            get
            {
                if (Items?.Count > 0)
                {
                    return Items.Sum(item => item.Total);
                }
                return 0;
            }
        }
    }

    public class CartItemViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Category")]
        public short CategoryId { get; set; }

        [Display(Name = "Manufacturer")]
        public short ManufacturerId { get; set; }

        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        public string Image { get; set; }

        [JsonIgnore]
        public decimal Total { get => Price * Quantity; }
    }
}
