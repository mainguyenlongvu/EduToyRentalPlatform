using ToyShop.Contract.Services.Interface;
using ToyShop.ModelViews.FeedBackModelViews;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ToyShop.Pages.Admin.FeedBack
{
    public class IndexModel : PageModel
    {
        private readonly IFeedBackService _feedBackService;

        public IndexModel(IFeedBackService feedBackService)
        {
            _feedBackService = feedBackService;
        }

        public IList<ResponeFeedBackModel> Feedbacks { get; set; }

        public int TotalItems { get; set; }  
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public async Task OnGetAsync(int pageNumber = 1, int pageSize = 10)
        {
            var feedbackList = await _feedBackService.GetFeedBacks_AdminAsync(pageNumber, pageSize, null);
            Feedbacks = feedbackList.Items.ToList();
            TotalItems = feedbackList.TotalItems;  
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
