using ToyShop.Contract.Services.Interface;
using ToyShop.ModelViews.FeedBackModelViews;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ToyShop.Pages.Admin.FeedBack
{
    public class DeleteModel : PageModel
    {
        private readonly IFeedBackService _feedBackService;

        public DeleteModel(IFeedBackService feedBackService)
        {
            _feedBackService = feedBackService;
        }

        public ResponeFeedBackModel Feedback { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id), "Feedback ID cannot be null or empty.");
            }

            Feedback = await _feedBackService.GetFeedBackAsync(id);
            if (Feedback == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            await _feedBackService.DeleteFeedBackAsync(id);
            return RedirectToPage("Index");
        }
    }
}
