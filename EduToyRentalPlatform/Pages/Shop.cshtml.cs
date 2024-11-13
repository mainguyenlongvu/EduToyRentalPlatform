using ToyShop.Contract.Services.Interface;
using ToyShop.ModelViews.ToyModelViews;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToyShop.ModelViews.ContractDetailModelView;

namespace ToyShop.Pages
{
    public class ShopModel : PageModel
    {
        private readonly IToyService _toyService;
        private readonly IContractService _contractService;
        private readonly IContractDetailService _contractDetailService;
        public ShopModel(IToyService toyService, IContractService contractService, IContractDetailService contractDetailService)
        {
            _toyService = toyService;
            _contractDetailService = contractDetailService;
            _contractService = contractService;
        }
        [BindProperty]
        public string ToyId { get; set; }

        [BindProperty]
        public int Quantity { get; set; }

        public List<ResponeToyModel> Toys { get; set; } = new List<ResponeToyModel>();
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchName { get; set; }
        public async Task OnGetAsync(int pageNumber = 1, int pageSize = 4)
        {
            var toys = await _toyService.GetToysAsync(pageNumber, pageSize, true, SearchName);

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
        public async Task<IActionResult> OnPostAddToCartAsync()
        {
            // Check if ToyId and Quantity have valid data
            if (!string.IsNullOrEmpty(ToyId) && Quantity > 0)
            {
                var model = new CreateContractDetailModel
                {
                    ToyId = ToyId,
                    Quantity = Quantity,
                    ContractType = true
                };

                await _contractDetailService.CreateContractDetailAsync(model);

                // Redirect to the Cart page after adding to cart
                return RedirectToPage("/Cart/Cart");
            }

            // Return to the current page if data is invalid
            return Page();
        }
    }
}
