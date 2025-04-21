using System.ComponentModel.DataAnnotations;

namespace BackendPractice.ProductModel
{
    public class ProductUIModel
    {
        public int Id { get; set; } // Unique identifier for the product
        public string? Category { get; set; } // Category of the product
        public string? PName { get; set; } // Unique product name
        public decimal Price { get; set; } // Price of the product
        public int Quantity { get; set; } // Available quantity of the product
        public string? Features { get; set; } // Features or details of the product
        public IFormFile Photo { get; set; } // File path to the product image


    }
}
