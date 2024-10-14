using ToyShop.Contract.Services.Interface;
using ToyShop.ModelViews.FeedBackModelViews;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ToyShop.Pages.Admin.FeedBack
{
    public class DetailsModel : PageModel
    {
        private readonly IFeedBackService _feedBackService;

        public DetailsModel(IFeedBackService feedBackService)
        {
            _feedBackService = feedBackService;
        }

        public ResponeFeedBackModel Feedback { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Lấy thông tin phản hồi từ service dựa trên id
            var responseFeedback = await _feedBackService.GetFeedBackAsync(id);

            if (responseFeedback == null)
            {
                return NotFound();
            }

            Feedback = responseFeedback; // Bind dữ liệu vào model

            return Page(); // Trả về trang Details
        }
    }
}
