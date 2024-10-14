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
    public class IndexModel : PageModel
    {
        private readonly ToyShop.Repositories.Base.ToyShopDBContext _context;

        public IndexModel(ToyShop.Repositories.Base.ToyShopDBContext context)
        {
            _context = context;
        }

        public IList<ContractEntity> ContractEntity { get;set; } = default!;

        public async Task OnGetAsync()
        {
            //ContractEntity = await _context.ContractEntitys
            //    .Include(c => c.ApplicationUser)
            //    .Include(c => c.Toy).ToListAsync();
        }
    }
}
