using ShoppingCart.Infrastructure.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCart.Models
{
    public class Product
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Please Enter a Valid Name")]
        public string Name { get; set; }
        public string Slug { get; set; }
        [Required,MinLength(5,ErrorMessage = "Minimun Length is 5")]
        public string Description { get; set; }
        [Required]
        [Range(0.1,double.MaxValue, ErrorMessage = "Range 0.1 - double.MaxValue")]
        [Column(TypeName = "decimal(8,2)")]
        public decimal Price { get; set; }
        public string Image { get; set; } = "noimage.png";

        [Required,Range(1, int.MaxValue, ErrorMessage = "You must choose a category")]
        public long CategoryId { get; set; }

        public Category Category { get; set; }

        [NotMapped]
        [FileExtension]
        public IFormFile ImageUpload { get; set; }

    }
}
