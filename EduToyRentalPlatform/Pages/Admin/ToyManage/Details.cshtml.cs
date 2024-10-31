using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ToyShop.Repositories.Base;
using ToyShop.ModelViews.ToyModelViews;
using ToyShop.Contract.Repositories.Entity;

namespace EduToyRentalPlatform.Pages.Admin.ToyManage
{
    public class DetailsModel : PageModel
    {
        private readonly ToyShopDBContext _context;

        public DetailsModel(ToyShopDBContext context)
        {
            _context = context;
        }

        public Toy? Toy { get; set; }

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            Toy = await _context.Toys
                .Where(t => t.Id == id)
                .Select(t => new Toy
                {
                    Id = t.Id,
                    ToyName = t.ToyName,
                    ToyImg = t.ToyImg,  // Map ToyImg property
                    ToyDescription = t.ToyDescription,
                    ToyPriceRent = t.ToyPriceRent,
                    ToyPriceSale = t.ToyPriceSale,
                    ToyRemainingQuantity = t.ToyRemainingQuantity,
                    ToyQuantitySold = t.ToyQuantitySold,
                    Option = t.Option
                })
                .FirstOrDefaultAsync();

            if (Toy == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
