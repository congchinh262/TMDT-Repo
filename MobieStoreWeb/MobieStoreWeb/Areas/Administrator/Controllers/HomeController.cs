using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MobieStoreWeb.Areas.Administrator.Models;
using MobieStoreWeb.Data;
using MobieStoreWeb.Helpers;
using MobieStoreWeb.Models;

namespace MobieStoreWeb.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [Authorize(Roles = "Admin,Employee")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.PaymentStatuses = SelectListHelper.GetEnumrableList<PaymentStatus>().Where(e => e.Value != PaymentStatus.Unpaid.ToString() && e.Value != PaymentStatus.Voided.ToString()).ToList();
            return View();
        }

        // GET: /Administrator/Home/GetRevenueMonths
        public async Task<IActionResult> GetRevenueMonths(DateTime? toDate,IEnumerable<PaymentStatus> paymentStatuses, bool withCurrentMonth = true, int prevMonthCount = 12)
        {
            if (prevMonthCount > 24)
            {
                prevMonthCount = 24;
            }
            if (!toDate.HasValue)
            {
                toDate = DateTime.Today;
            }
            var fromDate = new DateTime(toDate.Value.Year, toDate.Value.Month, 1).AddMonths(withCurrentMonth ? -prevMonthCount + 1 : -prevMonthCount);
            var filterOrders = _context.Orders.Where(o => o.OrderDate >= fromDate && o.Status != OrderStatus.Cancelled);
            if(paymentStatuses?.Count() > 0)
            {
                filterOrders = filterOrders.Where(o => paymentStatuses.Contains(o.PaymentStatus));
            }
            var filterOrdersWithTotal = await filterOrders
                .Include(o => o.OrderDetails)
                .Select(o => new
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    Total = o.OrderDetails.Sum(od => od.Price * od.Quantity)
                })
                .OrderBy(o => o.OrderDate)
                .AsNoTracking()
                .ToListAsync();

            var groupByMonthAndYear = filterOrdersWithTotal
                .GroupBy(o => new { o.OrderDate.Month, o.OrderDate.Year })
                .Select(g => new RevenueMonth
                {
                    Month = g.Key.Month,
                    Year = g.Key.Year,
                    Revenue = g.Sum(o => o.Total)
                });

            var revenueMonths = new List<RevenueMonth>();

            for (int i = 0; i < prevMonthCount; i++)
            {
                var newDateTime = fromDate.AddMonths(i);
                revenueMonths.Add(new RevenueMonth
                {
                    Month = newDateTime.Month,
                    Year = newDateTime.Year,
                    Revenue = groupByMonthAndYear.FirstOrDefault(g => g.Month == newDateTime.Month && g.Year == newDateTime.Year)?.Revenue ?? 0
                });
            }

            return Ok(revenueMonths);
        }
    }
}