using ToyShop.Contract.Services.Interface;
using ToyShop.ModelViews.ToyModelViews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ToyShop.Pages.Home
{
    //[Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly IToyService _toyService;

        public IndexModel(IToyService toyService)
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
