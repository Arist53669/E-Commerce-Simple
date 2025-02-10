using E_Commerce_Simple.Models;

namespace E_Commerce_Simple.ViewModels
{
    public class CheckoutViewModel
    {
        public List<Address> Addresses { get; set; }
        public Address Address { get; set; }
        public List<Cart> Carts { get; set; }
    }
}
