using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToyShop.Contract.Services.Interface;
using ToyShop.ModelViews.ContractDetailModelView;

namespace ToyShop.Pages.Shared
{
    public class ToyListPartialModel : PageModel
    {
        private readonly IContractService _contractService;

        public ToyListPartialModel(IContractService contractService)
        {
            _contractService = contractService;
        }

        public void OnGet()
        {
        }

        // Handler for adding item to cart
        public async Task<IActionResult> OnPostAddToCartAsync(string toyId)
        {
            var model = new CreateContractDetailModel
            {
                ToyId = toyId,
                Quantity = 1,
                ContractType = true
            };

            await _contractService.CreateContractDetailAsync(model);

            // Redirect to Cart page
            return RedirectToPage("/Cart");
        }

    }
}
