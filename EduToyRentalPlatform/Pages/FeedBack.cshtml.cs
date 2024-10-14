using ToyShop.Contract.Services.Interface;
using ToyShop.ModelViews.FeedBackModelViews;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ToyShop.Pages
{
    public class FeedBackModel : PageModel
    {
        private readonly IFeedBackService _feedBackService;

        public FeedBackModel(IFeedBackService feedBackService)
        {
            _feedBackService = feedBackService;
        }

        [BindProperty]
        public CreateFeedBackModel Feedback { get; set; }
        public List<ResponeFeedBackModel> Feedbacks { get; set; } = new List<ResponeFeedBackModel>();

        // Phân trang
        public int TotalItems { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }

        public async Task OnGetAsync(int pageNumber = 1, int pageSize = 4)
        {
            var feedbackList = await _feedBackService.GetFeedBacksAsync(pageNumber, pageSize, null);
            Feedbacks = feedbackList.Items.ToList();
            TotalItems = feedbackList.TotalItems;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = feedbackList.TotalPages; // Set total pages
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
            return RedirectToPage("FeedBack");
        }
    }
}
