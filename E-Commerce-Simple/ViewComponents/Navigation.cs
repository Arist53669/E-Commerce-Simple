using E_Commerce_Simple.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Simple.ViewComponents
{
    public class Navigation(ApplicationDbContext context) : ViewComponent
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IViewComponentResult> InvokeAsync()
        {
            int totalCart = await _context.Carts.CountAsync();

            ViewBag.TotalCart = totalCart;

            return View("Index");
        }
    }
}
