using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobieStoreWeb.Data;
using MobieStoreWeb.Helpers;
using MobieStoreWeb.ViewModels;

namespace MobieStoreWeb.Controllers
{
    public class CartController : Controller
    {
        private const string SessionKeyCart = "_Cart";
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult AddToCart(int id, int? quantity)
        {
            var product = _context.Products.Find(id);
            quantity = !quantity.HasValue ? 1 :
                        quantity > 5 ? 5 :
                        quantity < 1 ? 1 :
                        quantity.Value;
            if (product == null)
            {
                return NotFound();
            }

            var cart = HttpContext.Session.Get<CartViewModel>(SessionKeyCart);
            if (cart == null)
            {
                cart = new CartViewModel();
            }

            var item = cart.Items.SingleOrDefault(i => i.Id == product.Id);
            if (item == null)
            {
                cart.Items.Add(new CartItemViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    CategoryId = product.CategoryId,
                    ManufacturerId = product.ManufacturerId,
                    Size=product.Size,
                    Price = product.Price,
                    Quantity = quantity.Value,
                    Image = product.Image,
                });
            }
            else
            {
                item.Quantity+=quantity.Value;
                if (item.Quantity > 5)
                {
                    item.Quantity = 5;
                }
            }
            HttpContext.Session.Set(SessionKeyCart, cart);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public IActionResult RemoveFromCart(int id)
        {
            var cart = HttpContext.Session.Get<CartViewModel>(SessionKeyCart);
            var item = cart.Items.SingleOrDefault(i => i.Id == id);
            cart.Items.Remove(item);
            HttpContext.Session.Set(SessionKeyCart, cart);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Index()
        {
            var cart = HttpContext.Session.Get<CartViewModel>(SessionKeyCart);
            if (cart == null)
            {
                cart = new CartViewModel();
                HttpContext.Session.Set(SessionKeyCart, cart);
            }
            return View(cart);
        }

        [HttpPost]
        public IActionResult Index(CartViewModel viewModel) // Update Quantity
        {
            var cart = HttpContext.Session.Get<CartViewModel>(SessionKeyCart);
            cart.Items.ForEach(item =>
            {
                item.Quantity = viewModel.Items.FirstOrDefault(itemVM => itemVM.Id == item.Id)?.Quantity ?? item.Quantity;
                if (item.Quantity < 1)
                {
                    item.Quantity = 1;
                }
                if (item.Quantity > 5)
                {
                    item.Quantity = 5;
                }
            });
            HttpContext.Session.Set(SessionKeyCart, cart);
            return View(cart);
        }
    }
}