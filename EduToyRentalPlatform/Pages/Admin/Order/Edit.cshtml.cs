using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToyShop.Contract.Repositories.Entity;
using ToyShop.Repositories.Base;

namespace ToyShopRentalPlatform.Pages.Admin.Order
{
    public class EditModel : PageModel
    {
        private readonly ToyShop.Repositories.Base.ToyShopDBContext _context;

        public EditModel(ToyShop.Repositories.Base.ToyShopDBContext context)
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

            var contractentity =  await _context.ContractEntitys.FirstOrDefaultAsync(m => m.Id == id);
            if (contractentity == null)
            {
                return NotFound();
            }
            ContractEntity = contractentity;
           ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "FullName");
           ViewData["ToyId"] = new SelectList(_context.Toys, "Id", "Id");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(ContractEntity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContractEntityExists(ContractEntity.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ContractEntityExists(string id)
        {
            return _context.ContractEntitys.Any(e => e.Id == id);
        }
    }
}
