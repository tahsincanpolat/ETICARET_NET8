using ETICARET.Entities;
using System.ComponentModel.DataAnnotations;

namespace ETICARET.WebUI.Models
{
    public class ProductModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(60,MinimumLength = 10, ErrorMessage = "Ürün ismi min 10 max 60 karakter olmalıdır.")]
        public string Name { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 10, ErrorMessage = "Ürün açıklaması min 10 max 100 karakter olmalıdır.")]
        public string Description { get; set; }

        public List<Image>? Images { get; set; }

        [Required]
        public decimal Price { get; set; }

        public ProductModel()
        {
            Images = new List<Image>(); 
        }

        public List<Category> SelectedCategories { get; set; }
        public string CategoryId { get; set; }
    }
}
