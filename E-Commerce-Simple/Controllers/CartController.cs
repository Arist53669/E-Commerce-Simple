using E_Commerce_Simple.Data;
using E_Commerce_Simple.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Simple.Controllers
{
    //[Authorize]
    public class CartController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IActionResult> Index()
        {
            List<Cart> cart = await _context.Carts.Include("Product").ToListAsync();

            double totalPrice = 0;

            foreach (var cartItem in cart)
            {
                totalPrice += cartItem.Product.Price * cartItem.Quantity;
            }

            ViewBag.TotalPrice = totalPrice;

            return View(cart);
        }

        public async Task<IActionResult> AddToCart(int productId)
        {
            Product product = await _context.Products.FindAsync(productId);

            if (product == null)
            {
                return BadRequest();
            }

            Cart cart = new()
            {
                ProductId = productId,
                Quantity = 1
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
