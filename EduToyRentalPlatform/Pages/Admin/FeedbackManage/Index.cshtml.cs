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
        private readonly IFeedBackService _feedbackService;

        public IndexModel(IFeedBackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public IList<ResponeFeedBackModel> FeedBack { get; set; } = default!;

        public async Task OnGetAsync(int pageNumber = 1)
        {
            // Adjust the feedback retrieval logic to include pagination
            var feedbacks = await _feedbackService.GetFeedBacksAsync(pageNumber, TotalPages, true);

            FeedBack = feedbacks.Items.ToList();
            PageNumber = feedbacks.CurrentPage;
            TotalPages = feedbacks.TotalPages;
        }
    }
}
