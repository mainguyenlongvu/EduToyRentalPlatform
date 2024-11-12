using ToyShop.Contract.Repositories.Entity;
using ToyShop.ModelViews.ToyModelViews;

namespace EduToyRentalPlatform.Pages.Cart
{
    public class Item
    {
        public ContractEntity Contract { get; set; }
        public List<ContractDetail> Details { get; set; }
    }
}
