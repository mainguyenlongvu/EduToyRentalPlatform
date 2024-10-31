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
                return RedirectToPage("/Admin/ToyManage/Index");
            }
            return Page();
        }
    }
}
