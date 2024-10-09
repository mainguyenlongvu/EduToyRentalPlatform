using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ToyShop.Repositories.Base;
using ToyShop.Repositories.Entity;

namespace EduToyRentalPlatform.Pages.Admin.User
{
    public class IndexModel : PageModel
    {
        private readonly ToyShop.Repositories.Base.ToyShopDBContext _context;

        public IndexModel(ToyShop.Repositories.Base.ToyShopDBContext context)
        {
            _context = context;
        }

        public IList<ApplicationUser> ApplicationUser { get;set; } = default!;

        public async Task OnGetAsync()
        {
            //ApplicationUser = await _context.ApplicationUsers.ToListAsync();
        }
    }
}
