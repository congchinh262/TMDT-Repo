using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobieStoreWeb.Data;
using MobieStoreWeb.Helpers;
using MobieStoreWeb.Models;

namespace MobieStoreWeb.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;


        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string search, List<int> categories, List<int> manufacturers, int? page = 1)
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Manufacturers = await _context.Manufacturers.ToListAsync();
            var products = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Manufacturer)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                products = products.Where(p => p.Name.Contains(search) ||
                p.Category.Name.Contains(search) ||
                p.Manufacturer.Name.Contains(search));
            }

            if (categories?.Count > 0)
            {
                products = products.Where(p => categories.Contains(p.CategoryId));
            }
            if (manufacturers?.Count > 0)
            {
                products = products.Where(p => manufacturers.Contains(p.ManufacturerId));
            }
            return View(await PaginatedList<Product>.CreateAsync(products, page.Value, 12));
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Manufacturer)
                .FirstOrDefaultAsync(p => p.Id == id);
            if(product == null)
            {
                return NotFound();
            }
            return View(product);
        }
      
    }
}