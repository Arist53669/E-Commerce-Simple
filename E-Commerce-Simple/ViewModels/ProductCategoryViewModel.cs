using E_Commerce_Simple.Models;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Simple.ViewModels
{
    public class ProductCategoryViewModel
    {
        public Product Product { get; set; }
        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set; }

        [Display(Name = "Category")]
        public int? SelectedCategoryId { get; set; }
    }
}
