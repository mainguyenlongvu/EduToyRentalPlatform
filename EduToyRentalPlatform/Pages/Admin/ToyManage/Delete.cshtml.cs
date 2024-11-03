using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ToyShop.Contract.Repositories.Entity;
using ToyShop.Repositories.Base;

namespace EduToyRentalPlatform.Pages.Admin.ToyManage
{
    public class DeleteModel : PageModel
    {
        private readonly ToyShopDBContext _context;

        public DeleteModel(ToyShopDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Toy? Toy { get; set; }  // Add the Toy property

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            Toy = await _context.Toys.FirstOrDefaultAsync(m => m.Id == id);

            if (Toy == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            Toy = await _context.Toys.FindAsync(id);

            if (Toy != null)
            {
                _context.Toys.Remove(Toy);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("Admin/ToyManage/Index");
        }
    }
}
