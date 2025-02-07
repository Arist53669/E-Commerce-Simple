using E_Commerce_Simple.Data;
using E_Commerce_Simple.Models;
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
            return View(await _context.Products.OrderByDescending(p => p.UpdatedAt).ToListAsync());
        }

        // GET: Admin/Products/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Admin/Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name, Description, Price")] Product product, IFormFile img)
        {
            if (ModelState.IsValid)
            {
                if (img == null)
                {
                    ModelState.AddModelError(nameof(Product.Image), "Image is required.");
                    return View(product);
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

                product.CreatedAt = DateTime.Now;
                product.UpdatedAt = DateTime.Now;
                product.Quantity = 1;
                product.Image = $"/Admin/img/products/{imgName}";

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(product);
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
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product, IFormFile img)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Product oldProduct = await _context.Products.FindAsync(id);

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

                    oldProduct.Name = product.Name;
                    oldProduct.Description = product.Description;
                    oldProduct.Price = product.Price;
                    oldProduct.UpdatedAt = DateTime.Now;

                    _context.Update(oldProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            return View(product);
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
