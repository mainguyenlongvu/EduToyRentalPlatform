using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToyShop.Contract.Services.Interface;

namespace EduToyRentalPlatform.Pages.Admin.FeedbackManage
{
    public class DeleteModel : PageModel
    {
        private readonly IFeedBackService _feedBackService;

        public DeleteModel(IFeedBackService feedBackService)
        {
            _feedBackService = feedBackService;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            // Attempt to delete feedback
            bool isDeleted = await _feedBackService.DeleteFeedBackAsync(id);

            // Redirect to the Index page if deletion was successful
            if (isDeleted)
            {
                return RedirectToPage("Index");
            }

            // If deletion failed, return to the same page (could also show an error message)
            return Page();
        }
    }
}
