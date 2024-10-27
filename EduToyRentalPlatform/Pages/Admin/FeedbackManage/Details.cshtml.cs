using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ToyShop.Contract.Repositories.Entity;
using ToyShop.Repositories.Base;

namespace EduToyRentalPlatform.Pages.Admin.FeedbackManage
{
    public class DetailsModel : PageModel
    {
        private readonly ToyShop.Repositories.Base.ToyShopDBContext _context;

        public DetailsModel(ToyShop.Repositories.Base.ToyShopDBContext context)
        {
            _context = context;
        }

        public FeedBack FeedBack { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feedback = await _context.Feedbacks.FirstOrDefaultAsync(m => m.Id == id);
            if (feedback == null)
            {
                return NotFound();
            }
            else
            {
                FeedBack = feedback;
            }
            return Page();
        }
    }
}
