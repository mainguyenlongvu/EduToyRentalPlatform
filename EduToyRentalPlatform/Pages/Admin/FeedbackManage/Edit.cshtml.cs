using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToyShop.Contract.Repositories.Entity;
using ToyShop.Contract.Services.Interface;
using ToyShop.ModelViews.FeedBackModelViews;
using ToyShop.Repositories.Base;

namespace EduToyRentalPlatform.Pages.Admin.FeedbackManage
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
                Content = Feedback.Content
            };

            // Gọi phương thức cập nhật
            await _feedBackService.UpdateFeedBackAsync(Feedback.Id, feedbackToUpdate);
            return RedirectToPage("Index");
        }
    }
}
