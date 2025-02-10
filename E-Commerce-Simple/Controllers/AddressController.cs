using E_Commerce_Simple.Data;
using E_Commerce_Simple.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Simple.Controllers
{
    [Authorize]
    public class AddressController(ApplicationDbContext context, UserManager<AppUser> userManager) : Controller
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<AppUser> _userManager = userManager;

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("City, District, CommuneOrWard, SpecificAddress")] Address address)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                address.UserId = user.Id;

                await _context.Addresses.AddAsync(address);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index", "Checkout", address);
        }

        public async Task<IActionResult> SelectAddress(int id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var addresses = await _context.Addresses.Where(a => a.UserId == user.Id).ToListAsync();
            var address = addresses.Find(a => a.Id == id);

            if (address == null)
            {
                return BadRequest();
            }

            addresses.ForEach(address => address.IsSelected = false);
            address.IsSelected = true;

            _context.Addresses.Update(address);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Checkout");
        }
    }
}
