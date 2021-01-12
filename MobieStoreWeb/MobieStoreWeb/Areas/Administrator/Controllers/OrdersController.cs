using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MobieStoreWeb.Data;
using MobieStoreWeb.Helpers;
using MobieStoreWeb.Models;

namespace MobieStoreWeb.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [Authorize(Roles = "Admin,Employee")]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(/*string searchName,*/ OrderStatus? status, PaymentMethod? paymentMethod, PaymentStatus? paymentStatus, DeliveryOption? deliveryOption)
        {

            var orders = _context.Orders.AsQueryable();

            if (status.HasValue)
            {
                orders = orders.Where(o => o.Status == status);
            }
            if (paymentMethod.HasValue)
            {
                orders = orders.Where(o => o.PaymentMethod == paymentMethod);
            }
            if (paymentStatus.HasValue)
            {
                orders = orders.Where(o => o.PaymentStatus == paymentStatus);
            }
            if (deliveryOption.HasValue)
            {
                orders = orders.Where(o => o.DeliveryOption == deliveryOption);
            }
            //if (!string.IsNullOrWhiteSpace(searchName))
            //{
            //    orders = orders.Where(o => o.BillingName.Contains(searchName));
            //}

           
            ViewBag.Status = new SelectList(SelectListHelper.GetEnumrableList<OrderStatus>(), "Value", "Text", status);
            ViewBag.paymentMethod = new SelectList(SelectListHelper.GetEnumrableList<PaymentMethod>(), "Value", "Text", paymentMethod);
            ViewBag.PaymentStatus = new SelectList(SelectListHelper.GetEnumrableList<PaymentStatus>(), "Value", "Text", paymentStatus);
            ViewBag.DeliveryOption = new SelectList(SelectListHelper.GetEnumrableList<DeliveryOption>(), "Value", "Text", deliveryOption);
            //ViewBag.SearchName = searchName;
            return View(orders);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id, [Bind("Id,Name")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.OrderDetails
                .FirstOrDefaultAsync(o => o.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }
        private bool OrderExists(int id)
        {
            return _context.Orders.Any(o => o.Id == id);
        }
    }
}