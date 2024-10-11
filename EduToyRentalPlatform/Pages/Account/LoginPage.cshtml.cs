using ToyShop.Contract.Repositories.Entity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using ToyShop.Contract.Services.Interface;
using ToyShop.ModelViews.UserModelViews;


namespace ToyShop.Pages.Account
{
    public class LoginPageModel : PageModel
    {
        private readonly IUserService _userService;

        public LoginPageModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        [Required(ErrorMessage = "Hãy nhập Email tài khoản.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Hãy nhập mật khẩu.")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }
        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPostAsync()
        {


            if (ModelState.IsValid)
            {
                try
                {
                    var loginModel = new LoginModel
                    {
                        Email = Email,
                        Password = Password
                    };

                    string redirectUrl = await _userService.LoginAsync(loginModel);

                    Response.Cookies.Append("UserName", Email, new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddDays(7),
                        HttpOnly = true
                    });

                    // Chuyển hướng đến đường dẫn nhận được
                    return Redirect(redirectUrl);
                }
                catch (Exception ex)
                {
                    ErrorMessage = ex.Message;
                }
            }

            return Page();
        }
    }
}
