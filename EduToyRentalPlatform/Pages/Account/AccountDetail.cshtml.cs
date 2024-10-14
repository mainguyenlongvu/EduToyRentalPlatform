using ToyShop.Contract.Repositories.Entity;
using ToyShop.Contract.Services.Interface;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ToyShop.Contract.Services.Interface;
using ToyShop.Repositories.Entity;

namespace ToyShop.Pages.Account
{
    public class AccountDetailModel : PageModel
    {
        private readonly IUserService _userService;

        public AccountDetailModel(IUserService userService)
        {
            _userService = userService;
        }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string ErrorMessage { get; set; }

        public async Task OnGetAsync()
        {

            UserId = Request.Query["id"].ToString();

            if (!string.IsNullOrEmpty(UserId))
            {
                // Thiết lập cookie cho UserId
                Response.Cookies.Append("UserId", UserId, new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(7),
                    HttpOnly = true
                });

                // Gọi phương thức để lấy thông tin người dùng từ cơ sở dữ liệu
                User = await _userService.GetUserByIdAsync(UserId);
                if (User == null)
                {
                    ErrorMessage = "Không tìm thấy người dùng.";
                }
            }
            else
            {
                ErrorMessage = "ID người dùng không hợp lệ.";
            }
        }
    }
}
