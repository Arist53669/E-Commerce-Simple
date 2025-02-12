using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Simple.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(1, double.MaxValue, ErrorMessage = "Price must be number.")]
        [DataType(DataType.Currency)]
        public double Price { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string Image { get; set; }

        public int? CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
