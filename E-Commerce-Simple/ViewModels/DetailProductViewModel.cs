using E_Commerce_Simple.Models;

namespace E_Commerce_Simple.ViewModels
{
    public class DetailProductViewModel
    {
        public Product Product { get; set; }
        public int Qty { get; set; } = 1;
    }
}
