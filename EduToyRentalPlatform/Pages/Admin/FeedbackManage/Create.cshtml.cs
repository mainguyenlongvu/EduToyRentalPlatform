using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ToyShop.Contract.Repositories.Entity;
using ToyShop.Repositories.Base;

namespace EduToyRentalPlatform.Pages.Admin.FeedbackManage
{
    public class CreateModel : PageModel
    {
        private readonly ToyShop.Repositories.Base.ToyShopDBContext _context;

        public CreateModel(ToyShop.Repositories.Base.ToyShopDBContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["ToyId"] = new SelectList(_context.Toys, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public FeedBack FeedBack { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Feedbacks.Add(FeedBack);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
