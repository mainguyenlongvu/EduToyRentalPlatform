using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToyShop.ModelViews.UserModelViews; // Namespace of ApplicationDbContext
using ToyShop.Repositories.Base;
using ToyShop.Repositories.Entity;
using ToyShop.Contract.Repositories.Entity;
using ToyShop.Contract.Services.Interface;

namespace ToyShop.Pages.Account
{
    public class AccountDetailModel : PageModel
    {
        private readonly ToyShopDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public string UserName { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }

        public AccountDetailModel(ToyShopDBContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            // Get the UserId from cookies
            var userIdString = HttpContext.Request.Cookies["UserId"];
            if (userIdString == null)
            {
                return RedirectToPage("/Account/Login");
            }

            // Parse the UserId to Guid
            if (!Guid.TryParse(userIdString, out Guid userId))
            {
                return RedirectToPage("/Account/Login"); // Redirect if parsing fails
            }

            // Find the user by UserId
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
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
            // Get the UserId from cookies
            var userId = HttpContext.Request.Cookies["UserId"];
            if (userId == null)
            {
                return RedirectToPage("/Account/Login");
            }

            var user = await _userManager.FindByIdAsync(userId);
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
