using E_Commerce_Simple.Data;
using E_Commerce_Simple.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Simple.Controllers
{
    [Authorize]
    public class CartController(ApplicationDbContext context, UserManager<AppUser> userManager) : Controller
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<AppUser> _userManager = userManager;

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            List<Cart> cart = await _context.Carts.Where(c => c.UserId == user.Id).Include("Product").ToListAsync();

            double totalPrice = 0;

            foreach (var cartItem in cart)
            {
                totalPrice += cartItem.Product.Price * cartItem.Quantity;
            }

            ViewBag.TotalPrice = totalPrice;

            return View(cart);
        }

        public async Task<IActionResult> AddToCart(int productId, int qty)
        {
            Product product = await _context.Products.FindAsync(productId);
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (product == null)
            {
                return BadRequest();
            }

            Cart cart = new()
            {
                ProductId = productId,
                Quantity = qty,
                UserId = user.Id
            };

            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> UpdateQuantity(int id, int qty)
        {
            Cart cart = await _context.Carts.FindAsync(id);

            if (cart == null)
            {
                return BadRequest();
            }

            cart.Quantity = qty;
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Remove(int cartId)
        {
            Cart cart = await _context.Carts.FindAsync(cartId);

            if (cart == null)
            {
                return BadRequest();
            }

            _context.Remove(cart);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
