using E_Commerce_Simple.Data;
using E_Commerce_Simple.Models;
using E_Commerce_Simple.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Simple.Controllers
{
    [Authorize]
    public class CheckoutController(ApplicationDbContext context, UserManager<AppUser> userManager) : Controller
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<AppUser> _userManager = userManager;

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var addresses = await _context.Addresses.Where(a => a.UserId == user.Id).ToListAsync();
            var carts = await _context.Carts.Where(c => c.UserId == user.Id).Include(nameof(Product)).ToListAsync();
            var address = addresses.Where(a => a.IsSelected == true).First();

            CheckoutViewModel model = new()
            {
                Addresses = addresses,
                Carts = carts,
                Address = address
            };

            double totalPrice = 0;

            foreach (var cartItem in carts)
            {
                totalPrice += cartItem.Product.Price * cartItem.Quantity;
            }

            ViewBag.TotalPrice = totalPrice;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrder()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var carts = await _context.Carts.Include(nameof(Product)).Where(c => c.UserId == user.Id).ToListAsync();
            var address = await _context.Addresses.Where(a => a.UserId == user.Id).Where(a => a.IsSelected == true).FirstAsync();

            double totalPrice = 0;

            foreach (var cartItem in carts)
            {
                totalPrice += cartItem.Product.Price * cartItem.Quantity;
            }

            Order order = new()
            {
                UserId = user.Id,
                OrderStatus = "wait",
                AddressId = address.Id,
                Amount = totalPrice,
                CreatedAt = DateTime.Now,
            };

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            foreach (var cartItem in carts)
            {
                OrderItem item = new()
                {
                    OrderId = order.Id,
                    ProductId = cartItem.ProductId
                };

                await _context.OrderItems.AddAsync(item);
            }

            _context.Carts.RemoveRange(carts);
            await _context.SaveChangesAsync();

            return RedirectToAction("Thanks");
        }

        public IActionResult Thanks()
        {
            return View();
        }
    }
}
