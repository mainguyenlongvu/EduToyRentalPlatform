using ToyShop.Contract.Services.Interface;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;
using ToyShop.Repositories.Entity;

namespace ToyShop.Pages.Account
{
    public class ProfileDetailModel : PageModel
    {
        private readonly IUserService _userService;

        public ProfileDetailModel(IUserService userService)
        {
            _userService = userService;
        }

        public string UserId { get; private set; }
        public ApplicationUser User { get; private set; }
        public string ErrorMessage { get; private set; }

        public async Task OnGetAsync()
        {
            // Get UserId from query parameters
            UserId = Request.Query["id"];

            if (string.IsNullOrEmpty(UserId))
            {
                ErrorMessage = "ID người dùng không hợp lệ.";
                return;
            }

            // Set a cookie for UserId
            Response.Cookies.Append("UserId", UserId, new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(7),
                HttpOnly = true
            });

            // Retrieve user information based on UserId
            User = await _userService.GetUserByIdAsync(UserId);

            if (User == null)
            {
                ErrorMessage = "Không tìm thấy người dùng.";
            }
        }
    }
}
