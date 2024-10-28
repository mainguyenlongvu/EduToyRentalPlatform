using EduToy.Contract.Services.Interface;
using EduToy.ModelViews.FeedBackModelViews;
using EduToy.ModelViews.ToyModelViews;
using EduToy.Services.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EduToy.Pages
{
    public class ShopDetailModel : PageModel
    {
        private readonly IToyService _toyService;
        private readonly IFeedBackService _feedBackService;

        public ResponeToyModel Toy { get; set; }
        [BindProperty]
        public CreateFeedBackModel Feedback { get; set; }
        public List<ResponeFeedBackModel> Feedbacks { get; set; } = new List<ResponeFeedBackModel>();

        // Phân trang
        public int TotalItems { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }

        public ShopDetailModel(IToyService toyService, IFeedBackService feedBackService)
        {
            _toyService = toyService;
            _feedBackService = feedBackService;
        }

        public async Task<IActionResult> OnGetAsync(string id, int pageNumber = 1, int pageSize = 4)
        {
            Toy = await _toyService.GetToyAsync(id);
            if (Toy == null)
            {
                return NotFound();
            }

            var feedbackList = await _feedBackService.GetFeedBacksByToyIdAsync(id, pageNumber, pageSize, null);
            Feedbacks = feedbackList.Items.ToList();
            TotalItems = feedbackList.TotalItems;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = feedbackList.TotalPages;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || Feedback == null)
            {
                return Page();
            }

            var UserId = Request.Cookies.ContainsKey("UserId")
               ? Request.Cookies["UserId"]
               : null;

            if (string.IsNullOrEmpty(UserId))
            {
                TempData["ErrorMessage"] = "You'll need to sign in to send feedback.";
                return RedirectToPage("/Account/LoginPage"); // Redirect to the login page
            }

            var feedback = new CreateFeedBackModel
            {
                UserId = UserId,
                ToyId = Feedback.ToyId,
                Content = Feedback.Content
            };

            await _feedBackService.CreateFeedBackAsync(feedback);
            return RedirectToPage("ShopDetail", new { id = Feedback.ToyId });
        }
    }
}
