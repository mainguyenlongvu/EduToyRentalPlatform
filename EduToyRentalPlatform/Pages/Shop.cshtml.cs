using ToyShop.Contract.Services.Interface;
using ToyShop.ModelViews.ToyModelViews;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ToyShop.Pages
{
    public class ShopModel : PageModel
    {
        private readonly IToyService _toyService;

        public ShopModel(IToyService toyService)
        {
            _toyService = toyService;
        }

        public List<ResponeToyModel> ToysList { get; set; } = new List<ResponeToyModel>();

        public async Task OnGet()
        {
            var toys = await _toyService.GetToysAsync(1, 10, null);
            ToysList = toys.Items.ToList();
        }
    }
}
