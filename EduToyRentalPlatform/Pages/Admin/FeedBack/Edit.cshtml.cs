using ToyShop.Contract.Services.Interface;
using ToyShop.ModelViews.FeedBackModelViews;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace ToyShop.Pages.Admin.FeedBack
{
    public class EditModel : PageModel
    {
        private readonly IFeedBackService _feedBackService;

        public EditModel(IFeedBackService feedBackService)
        {
            _feedBackService = feedBackService;
        }
        [BindProperty]
        public ResponeFeedBackModel Feedback { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var responseFeedback = await _feedBackService.GetFeedBackAsync(id);

            if (responseFeedback == null)
            {
                return NotFound();
            }

            Feedback = responseFeedback;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || Feedback == null)
            {
                return Page();
            }

            // Tạo đối tượng feedbackToUpdate từ Feedback
            var feedbackToUpdate = new ResponeFeedBackModel
            {
                UserId = Feedback.UserId, // Sử dụng giá trị từ Feedback
                ToyId = Feedback.ToyId, // Sử dụng giá trị từ Feedback
                Content = Feedback.Content
            };

            // Gọi phương thức cập nhật
            await _feedBackService.UpdateFeedBackAsync(Feedback.Id, feedbackToUpdate);
            return RedirectToPage("Index");
        }
    }
}
