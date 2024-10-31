
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ToyShop.ModelViews.ToyModelViews
{
    public class ResponeToyModel
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "Toy name is required.")]
        public string? ToyName { get; set; }

        public string? ToyImg { get; set; }

        [Required(ErrorMessage = "Toy description is required.")]
        public string? ToyDescription { get; set; }

        [Required(ErrorMessage = "Sale price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Sale price must be a positive value.")]
        public int ToyPriceSale { get; set; }

        [Required(ErrorMessage = "Rent price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Rent price must be a positive value.")]
        public int ToyPriceRent { get; set; }

        [Required(ErrorMessage = "Remaining quantity is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Remaining quantity cannot be negative.")]
        public int ToyRemainingQuantity { get; set; }

        [Required(ErrorMessage = "Quantity sold is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity sold cannot be negative.")]
        public int ToyQuantitySold { get; set; }

        public string? Option { get; set; }

        public DateTimeOffset LastUpdatedTime { get; set; }

        public bool IsDeleted { get; set; }

        public IFormFile? ImageFile { get; set; }

    }
}
