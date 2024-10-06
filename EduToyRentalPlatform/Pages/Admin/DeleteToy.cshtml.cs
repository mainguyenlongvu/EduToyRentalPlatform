using ToyShop.Contract.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ToyShop.Pages.Admin
{
    public class DeleteToyModel : PageModel
    {

        private readonly IToyService _toyService;

        public DeleteToyModel(IToyService toyService)
        {
            _toyService = toyService;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            bool isDeleted = await _toyService.DeleteToyAsync(id);

            if (isDeleted == true)
            {
                return RedirectToPage("/Admin/Product");
            }

            return Page();
        }
        
    }
}
