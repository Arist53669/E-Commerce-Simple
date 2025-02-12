using E_Commerce_Simple.Data;
using E_Commerce_Simple.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Simple.Controllers
{
    public class ProductsController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IActionResult> Index(int? categoryId, int page = 1, int pageSize = 4)
        {
            var productsQuery = categoryId == null
                ? _context.Products.OrderByDescending(p => p.UpdatedAt)
                : _context.Products.Where(p => p.CategoryId == categoryId).OrderByDescending(p => p.UpdatedAt);

            var products = await productsQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            var categories = await _context.Category.Include(c => c.Products).ToListAsync();

            ProductCategoryViewModel vm = new()
            {
                Products = products,
                Categories = categories
            };

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = Math.Ceiling((double)productsQuery.Count() / pageSize);

            return View(vm);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
    }
}
