using System.ComponentModel.DataAnnotations;

namespace Day4.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Phai nhap ten danh muc")]
        [StringLength(100)]
        public string? Name { get; set; }

        public ICollection<Product>? Products { get; set; }
    }
}
