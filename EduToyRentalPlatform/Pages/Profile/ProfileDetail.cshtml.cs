using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using ToyShop.ModelViews.UserModelViews; // Namespace of ApplicationDbContext
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToyShop.Repositories.Base;
using ToyShop.Repositories.Entity;
using ToyShop.Contract.Repositories.Entity;
using ToyShop.Contract.Services.Interface;

namespace ToyShop.Pages.Account
{
    public class ProfileDetailModel : PageModel
    {
        private readonly ToyShopDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public string UserName { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }

        public ProfileDetailModel(ToyShopDBContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var userName = HttpContext.Request.Cookies["UserName"];
            if (userName == null)
            {
                return RedirectToPage("/Account/Login");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            if (user != null)
            {
                UserName = user.UserName;
                Email = user.Email;
                Phone = user.Phone;
            }
            else
            {
                UserName = "Guest";
                Email = "Not Available";
                Phone = "Not Available";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostBackAsync()
        {
            var userName = HttpContext.Request.Cookies["UserName"];
            if (userName == null)
            {
                return RedirectToPage("/Account/Login");
            }

            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            // Check if the user is in the "Admin" role
            bool isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

            if (isAdmin)
            {
                return RedirectToPage("/Admin/Index");
            }
            else
            {
                return RedirectToPage("/Shop");
            }
        }
    }
}
