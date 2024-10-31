using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ToyShop.ModelViews.ToyModelViews
{
    public class CreateToyModel
    {
        [Required(ErrorMessage = "You must enter a toy name!")]
        public string ToyName { get; set; } = string.Empty;

        public string? ToyImg { get; set; }

        public string? ToyDescription { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to 0!")]
        public decimal ToyPriceSale { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Rent price must be greater than or equal to 0!")]
        public decimal ToyPriceRent { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "The remaining quantity must be greater than or equal to 0!")]
        public int ToyRemainingQuantity { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "The number sold must be greater than or equal to 0!")]
        public int ToyQuantitySold { get; set; }

        [Required(ErrorMessage = "You must specify an option!")]
        public string? Option { get; set; }

        public bool IsDeleted { get; set; }

        [Required(ErrorMessage = "You must upload an image file!")]
        public IFormFile? ImageFile { get; set; }
    }
}
