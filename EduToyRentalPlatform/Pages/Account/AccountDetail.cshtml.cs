using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToyShop.ModelViews.UserModelViews;
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

        public string Email { get; private set; } // Email is non-editable

        // Editable properties
        [BindProperty]
        [Required(ErrorMessage = "User name is required.")]
        public string UserName { get; set; }

        [BindProperty]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string Phone { get; set; }

        [BindProperty]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [BindProperty]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        public AccountDetailModel(ToyShopDBContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            // Retrieve UserId from cookies
            var userIdString = HttpContext.Request.Cookies["UserId"];
            if (userIdString == null)
            {
                return RedirectToPage("/Account/Login");
            }

            if (!Guid.TryParse(userIdString, out Guid userId))
            {
                return RedirectToPage("/Account/Login");
            }

            // Load the user by UserId
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

        public async Task<IActionResult> OnPostUpdateAsync()
        {
            // Retrieve UserId from cookies
            var userIdString = HttpContext.Request.Cookies["UserId"];
            if (userIdString == null)
            {
                return RedirectToPage("/Account/Login");
            }

            if (!Guid.TryParse(userIdString, out Guid userId))
            {
                return RedirectToPage("/Account/Login");
            }

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            // Update user properties
            user.UserName = UserName;
            user.Phone = Phone;

            // Update password if a new one is provided
            if (!string.IsNullOrWhiteSpace(NewPassword))
            {
                var result = await _userManager.RemovePasswordAsync(user);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Failed to remove old password.");
                    return Page();
                }

                result = await _userManager.AddPasswordAsync(user, NewPassword);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Failed to set new password.");
                    return Page();
                }
            }

            // Save changes to the database
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to update user information.");
                return Page();
            }

            return RedirectToPage("/Account/AccountDetail");
        }

        public async Task<IActionResult> OnPostBackAsync()
        {
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

            bool isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

            return isAdmin ? RedirectToPage("/Admin/Index") : RedirectToPage("/Shop");
        }
    }
}
