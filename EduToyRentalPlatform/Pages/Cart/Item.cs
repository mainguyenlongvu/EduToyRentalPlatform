using ToyShop.Contract.Repositories.Entity;
using ToyShop.ModelViews.ToyModelViews;

namespace EduToyRentalPlatform.Pages.Cart
{
    public class Item
    {
        public ResponeToyModel Toy { get; set; }
        public int Quantity { get; set; }
    }
}
