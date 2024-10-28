
using Microsoft.AspNetCore.Http;

namespace ToyShop.ModelViews.ToyModelViews
{
    public class ResponeToyModel
    {
        public string? Id { get; set; }

        public string? ToyName { get; set; }
        public string? ToyImg { get; set; }
        public string? ToyDescription { get; set; }
        public int ToyPriceSale { get; set; }
        public int ToyPriceRent { get; set; }
        public int ToyRemainingQuantity { get; set; }
        public int ToyQuantitySold { get; set; }
        public string? option { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public bool IsDeleted { get; set; }

        public IFormFile? ImageFile { get; set; }

    }
}
