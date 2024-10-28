using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToyShop.Contract.Repositories.Entity;
using ToyShop.Contract.Services.Interface;
using ToyShop.ModelViews.FeedBackModelViews;

namespace EduToyRentalPlatform.Pages.Admin.FeedbackManage
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
        [BindProperty(SupportsGet = true)]
        public string SearchName { get; set; }

        public async Task OnGetAsync(int pageNumber = 1, int pageSize = 8)
        {
            var feedbackList = await _feedBackService.GetFeedBacks_AdminAsync(pageNumber, pageSize, null);

            if (!string.IsNullOrEmpty(SearchName))
            {
                Feedbacks = feedbackList.Items.Where(t => t.Content.Contains(SearchName, StringComparison.OrdinalIgnoreCase)).ToList();
                TotalItems = feedbackList.TotalItems;
                PageNumber = pageNumber;
                PageSize = pageSize;
            }
            else
            {
                Feedbacks = feedbackList.Items.ToList();
                TotalItems = feedbackList.TotalItems;
                PageNumber = pageNumber;
                PageSize = pageSize;
            }
        }
    }
}
