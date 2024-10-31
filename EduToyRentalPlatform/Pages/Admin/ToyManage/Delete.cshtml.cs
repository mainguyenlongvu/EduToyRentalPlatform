using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToyShop.Contract.Services.Interface;

namespace EduToyRentalPlatform.Pages.Admin.ToyManage
{
    public class DeleteModel : PageModel
    {
        private readonly IToyService _toyService;

        public DeleteModel(IToyService toyService)
        {
            _toyService = toyService;
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            bool isDeleted = await _toyService.DeleteToyAsync(id);

            if (isDeleted)
            {
                // Optionally add a success message here if using TempData for user notifications
                return RedirectToPage("/Admin/ToyManage/Index");
            }

            // If deletion fails, you can return an error message (consider adding a ModelState error or a TempData message)
            return Page();
        }
    }
}
