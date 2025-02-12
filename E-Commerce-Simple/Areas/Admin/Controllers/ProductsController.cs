using E_Commerce_Simple.Data;
using E_Commerce_Simple.Models;
using E_Commerce_Simple.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Simple.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        // GET: Admin/Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.Include(p => p.Category).OrderByDescending(p => p.UpdatedAt).ToListAsync());
        }

        // GET: Admin/Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Admin/Products/Create
        public async Task<IActionResult> Create()
        {
            var categories = await _context.Category.ToListAsync();

            ProductCategoryViewModel vm = new()
            {
                Categories = categories,
                SelectedCategoryId = categories.First().Id
            };

            return View(vm);
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCategoryViewModel vm, IFormFile img)
        {
            if (ModelState.IsValid)
            {
                if (img == null)
                {
                    ModelState.AddModelError(nameof(Product.Image), "Image is required.");
                    return View(vm);
                }

                string imgName = Guid.NewGuid() + Path.GetExtension(img.FileName).ToLower();
                string imgDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Admin/img/products");

                if (!Directory.Exists(imgDirectory))
                {
                    Directory.CreateDirectory(imgDirectory);
                }

                string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Admin/img/products", imgName);

                await using (FileStream stream = new(savePath, FileMode.Create))
                {
                    await img.CopyToAsync(stream);
                }

                Product product = new()
                {
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Image = $"/Admin/img/products/{imgName}",
                    Name = vm.Product.Name,
                    Description = vm.Product.Description,
                    Price = vm.Product.Price,
                    CategoryId = vm.SelectedCategoryId
                };

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(vm);
        }

        // GET: Admin/Products/Edit/5
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
            var categories = await _context.Category.ToListAsync();

            ProductCategoryViewModel vm = new()
            {
                Categories = categories,
                Product = product,
                SelectedCategoryId = product.CategoryId == null ? categories.First().Id : product.CategoryId
            };

            return View(vm);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductCategoryViewModel vm, IFormFile img)
        {
            Product oldProduct = await _context.Products.FindAsync(id);

            if (oldProduct == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (img != null)
                {
                    string imgName = Guid.NewGuid() + Path.GetExtension(img.FileName).ToLower();
                    string imgDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Admin/img/products");

                    if (!Directory.Exists(imgDirectory))
                    {
                        Directory.CreateDirectory(imgDirectory);
                    }

                    string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Admin/img/products", imgName);

                    await using (FileStream stream = new(savePath, FileMode.Create))
                    {
                        await img.CopyToAsync(stream);
                    }

                    oldProduct.Image = $"/Admin/img/products/{imgName}";
                }

                oldProduct.Name = vm.Product.Name;
                oldProduct.Description = vm.Product.Description;
                oldProduct.Price = vm.Product.Price;
                oldProduct.CategoryId = vm.SelectedCategoryId;
                oldProduct.UpdatedAt = DateTime.Now;

                _context.Update(oldProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        // GET: Admin/Products/Delete/5
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

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
