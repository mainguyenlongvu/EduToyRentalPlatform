using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ToyShop.Contract.Repositories.Entity;
using ToyShop.Contract.Services.Interface;
using ToyShop.Repositories.Base;

namespace EduToyRentalPlatform.Pages.Admin.FeedbackManage
{
    public class DeleteModel : PageModel
    {
        private readonly IFeedBackService _feedBackService;

        public DeleteModel(IFeedBackService feedBackService)
        {
            _feedBackService = feedBackService;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            bool isDeleted = await _feedBackService.DeleteFeedBackAsync(id);

            if (isDeleted == true)
            {
                return RedirectToPage("Index");
            }

            return Page();
        }
    }
}
