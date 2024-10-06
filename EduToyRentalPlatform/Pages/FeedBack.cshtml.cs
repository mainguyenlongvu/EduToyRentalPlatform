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

        public List<ResponeFeedBackModel> Feedbacks { get; set; } = new List<ResponeFeedBackModel>();
        public async Task OnGet()
        {
            Feedbacks = (await _feedBackService.GetFeedBacksAsync(1, int.MaxValue, null)).Items.ToList();
        }
    }
}
