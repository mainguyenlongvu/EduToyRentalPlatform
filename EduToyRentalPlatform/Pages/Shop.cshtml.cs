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

        public List<ResponeToyModel> Toys { get; set; } = new List<ResponeToyModel>();
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchName { get; set; }
        public async Task OnGetAsync(int pageNumber = 1, int pageSize = 4)
        {
            var toys = await _toyService.GetToysAsync(pageNumber, pageSize, null);

            if (!string.IsNullOrEmpty(SearchName))
            {
                Toys = toys.Items.Where(t => t.ToyName.Contains(SearchName, StringComparison.OrdinalIgnoreCase)).ToList();
                PageNumber = toys.CurrentPage;
                TotalPages = toys.TotalPages;
            }
            else
            {
                Toys = toys.Items.ToList();
                PageNumber = toys.CurrentPage;
                TotalPages = toys.TotalPages;
            }
        }
    }
}
