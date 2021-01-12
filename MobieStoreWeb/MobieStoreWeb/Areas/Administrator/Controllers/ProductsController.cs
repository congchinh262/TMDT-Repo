using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MobieStoreWeb.Areas.Administrator.ViewModels;
using MobieStoreWeb.Data;
using MobieStoreWeb.Helpers;
using MobieStoreWeb.Models;

namespace MobieStoreWeb.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [Authorize(Roles = "Admin,Employee")]
    public class ProductsController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _context;

        public ProductsController(IWebHostEnvironment webHostEnvironment, ApplicationDbContext context)
        {
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Manufacturer)
                .ToListAsync());
        }

        public async Task<IActionResult> Details(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.CategoryId = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name");
            ViewBag.ManufacturerId = new SelectList(await _context.Manufacturers.ToListAsync(), "Id", "Name");
            return View();
        }

        // POST: Administrator/Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel viewModel)
        {
            var extension = "";
            if (viewModel.ImageFile != null)
            {
                if (!viewModel.ImageFile.IsValidFileSizeLimit(26214400))
                {
                    ModelState.AddModelError("ImageFile", "File size must be less than 25 MiB.");
                }

                if (!viewModel.ImageFile.IsValidImageFileExtension(out extension))
                {
                    ModelState.AddModelError("ImageFile", "Invalid file extension.");
                }
            }

            if (ModelState.IsValid)
            {
                var product = new Product
                {
                    Id = viewModel.Id,
                    Name = viewModel.Name,
                    CategoryId = viewModel.CategoryId,
                    Quantity = viewModel.Quantity,
                    ManufacturerId = viewModel.ManufacturerId,
                    Size=viewModel.Size,
                    Description = viewModel.Description,
                    Price = viewModel.Price,
                    PublishDate = viewModel.PublishDate,
                    Status = viewModel.Status,
                };
                if (viewModel.ImageFile != null)
                {
                    var fileName = $"{Path.GetRandomFileName()}{extension}";
                    var savePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products");

                    using (var stream = new FileStream(Path.Combine(savePath, fileName), FileMode.Create))
                    {
                        await viewModel.ImageFile.CopyToAsync(stream);
                    }
                    product.Image = fileName;
                }
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CategoryId = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name", viewModel.CategoryId);
            ViewBag.ManufacturerId = new SelectList(await _context.Manufacturers.ToListAsync(), "Id", "Name", viewModel.ManufacturerId);
            return View(viewModel);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.CategoryId = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name", product.CategoryId);
            ViewBag.ManufacturerId = new SelectList(await _context.Manufacturers.ToListAsync(), "Id", "Name", product.ManufacturerId);
            return View(new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Size=product.Size,
                ManufacturerId = product.ManufacturerId,
                CategoryId = product.CategoryId,
                PublishDate = product.PublishDate,
                Quantity = product.Quantity,
                Status = product.Status,
                Description = product.Description,
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            var extension = "";
            if (viewModel.ImageFile != null)
            {
                if (!viewModel.ImageFile.IsValidFileSizeLimit(26214400))
                {
                    ModelState.AddModelError("ImageFile", "File size must be less than 25 MiB.");
                }

                if (!viewModel.ImageFile.IsValidImageFileExtension(out extension))
                {
                    ModelState.AddModelError("ImageFile", "Invalid file extension.");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Product product = _context.Products.FirstOrDefault(p => p.Id == id);
                    product.Id = viewModel.Id;
                    product.Name = viewModel.Name;
                    product.CategoryId = viewModel.CategoryId;
                    product.ManufacturerId = viewModel.ManufacturerId;
                    product.Size = viewModel.Size;
                    product.Price = viewModel.Price;
                    product.PublishDate = viewModel.PublishDate;
                    product.Quantity = viewModel.Quantity;
                    product.Status = viewModel.Status;

                    if (viewModel.ImageFile != null)
                    {
                        var fileName = $"{Path.GetRandomFileName()}{extension}";
                        var savePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products");

                        using (var stream = new FileStream(Path.Combine(savePath, fileName), FileMode.Create))
                        {
                            await viewModel.ImageFile.CopyToAsync(stream);
                        }
                        product.Image = fileName;
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(viewModel.Id))
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
            ViewBag.CategoryId = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name", viewModel.CategoryId);
            ViewBag.ManufacturerId = new SelectList(await _context.Manufacturers.ToListAsync(), "Id", "Name", viewModel.ManufacturerId);
            return View(viewModel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}