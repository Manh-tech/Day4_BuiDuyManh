
using Day4.Models;

namespace Day4.Data
{
    public static class SeedData
    {
        // Danh sach danh muc
        public static List<Category> categories = new List<Category>
        {
            new Category
            {
                Id = 1,
                Name = "Dien thoai"
            },
            new Category
            {
                Id = 2,
                Name = "Laptop"
            },
            new Category
            {
                Id = 3,
                Name = "Phu kien"
            }
        };

        // Danh sach san pham
        public static List<Product> products = new List<Product>
        {
            new Product
            {
                Id = 1,
                Name = "iPhone 15",
                Price = 20000000,
                Description = "Dien thoai Apple",
                CategoryId = 1
            },
            new Product
            {
                Id = 2,
                Name = "Dell XPS",
                Price = 25000000,
                Description = "Laptop Dell",
                CategoryId = 2
            },
            new Product
            {
                Id = 3,
                Name = "Chuot Logitech",
                Price = 500000,
                Description = "Chuot khong day",
                CategoryId = 3
            }
        };
    }
}
