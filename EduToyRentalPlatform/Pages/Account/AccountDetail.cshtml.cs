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
using System.Drawing;

namespace ToyShop.Pages.Account
{
    public class AccountDetailModel : PageModel
    {
        private readonly IUserService _userService;

        public AccountDetailModel(IUserService userService)
        {
            _userService = userService;
        }
        [BindProperty]
        public string UserName { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Phone { get; set; }
        public string Img { get; set; }
        [BindProperty]
        public string NewPassword { get; set; }

        [BindProperty]
        public string ConfirmPassword { get; set; }


        [BindProperty]
        public UpdateCustomerModel UserDetails { get; set; } = new UpdateCustomerModel();

        [BindProperty]
        public ChangPasswordModel ChangePassword { get; set; } = new ChangPasswordModel();

        public async Task<IActionResult> OnGetAsync(Guid userId)
        {
            // Load the user details
            var user = await _userService.GetUserByIdAsync(userId.ToString());

            if (user == null)
            {
                return NotFound();
            }

            // Populate UserDetails model with retrieved data, excluding email
            UserDetails.FullName = user.FullName;
            UserDetails.Phone = user.Phone;
            UserDetails.Email = user.Email;
            UserDetails.ImageUrl = user.ImageUrl;

            return Page();
        }

        public async Task<IActionResult> OnPostUpdateDetailsAsync(Guid userId)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Call UpdateCustomerAsync without changing email
            await _userService.UpdateCustomerAsync(userId, UserDetails);

            return RedirectToPage("AccountDetail", new { userId });
        }

        public async Task<IActionResult> OnPostChangePasswordAsync(Guid userId)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Call ChangePasswordAsync to change the password
            await _userService.ChangPasswordAsync(ChangePassword);

            return RedirectToPage("AccountDetail", new { userId });
        }
    }
}
