using ToyShop.Contract.Services.Interface;
using ToyShop.ModelViews.FeedBackModelViews;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ToyShop.Pages.FeedBack
{
    public class CreateModel : PageModel
    {
        private readonly IFeedBackService _feedBackService;

        public CreateModel(IFeedBackService feedBackService)
        {
            _feedBackService = feedBackService;
        }

        [BindProperty]
        public CreateFeedBackModel Feedback { get; set; }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || Feedback == null)
            {
                return Page();
            }

            var feedback = new CreateFeedBackModel
            {
                UserId = Feedback.UserId,
                ToyId = Feedback.ToyId,
                Content = Feedback.Content
            };

            await _feedBackService.CreateFeedBackAsync(feedback);
            return RedirectToPage("Index");
        }
    }
}
