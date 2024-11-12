using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToyShop.Contract.Repositories.Entity;

namespace EduToyRentalPlatform.Pages.Cart
{
    public class CheckoutModel : PageModel
    {
        public List<ContractDetail> CartItems { get; set; }

        public void OnGet()
        {
            // Retrieve the cart items from TempData
            if (TempData["CartItems"] != null)
            {
                CartItems = TempData["CartItems"] as List<ContractDetail>;
            }
        }
    }
}
