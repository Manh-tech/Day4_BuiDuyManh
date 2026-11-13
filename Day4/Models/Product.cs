
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Day4.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Phai nhap ten san pham")]
        [StringLength(100)]
        public string? Name { get; set; }

        [Range(0.01, 1000000000,
            ErrorMessage = "Gia san pham phai lon hon 0")]
        public decimal Price { get; set; }

        public string? Description { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public Category? Category { get; set; }
    }
}
