using System.ComponentModel.DataAnnotations;
using book.entity;

namespace book.webui.Models
{
    public class CategoryModel
    {
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Name alani mecburi alan")]
        [StringLength(60, MinimumLength = 5, ErrorMessage = " Name 5-60 arasinda simvol daxil etmeyiniz teleb olunur")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Url alani mecburi alan")]
        [StringLength(60, MinimumLength = 5, ErrorMessage = " Url 5-60 arasinda simvol daxil etmeyiniz teleb olunur")]

        public string? Url { get; set; }
        public List<Product>? Products { get; set; }

    }
}