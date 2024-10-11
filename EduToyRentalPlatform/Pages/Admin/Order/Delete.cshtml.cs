using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ToyShop.Contract.Repositories.Entity;
using ToyShop.Repositories.Base;

namespace ToyShopRentalPlatform.Pages.Admin.Order
{
    public class DeleteModel : PageModel
    {
        private readonly ToyShop.Repositories.Base.ToyShopDBContext _context;

        public DeleteModel(ToyShop.Repositories.Base.ToyShopDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ContractEntity ContractEntity { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contractentity = await _context.ContractEntitys.FirstOrDefaultAsync(m => m.Id == id);

            if (contractentity == null)
            {
                return NotFound();
            }
            else
            {
                ContractEntity = contractentity;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contractentity = await _context.ContractEntitys.FindAsync(id);
            if (contractentity != null)
            {
                ContractEntity = contractentity;
                _context.ContractEntitys.Remove(ContractEntity);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
