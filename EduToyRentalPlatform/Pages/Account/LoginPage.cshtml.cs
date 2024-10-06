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

        public string Email { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Hãy nhập mật khẩu.")]
        public string Password { get; set; }
        [BindProperty]
        public bool RememberMe { get; set; }


        public string ErrorMessage { get; set; }


        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                try
                {
                    // Tạo model LoginModel từ thông tin người dùng
                    var loginModel = new LoginModel
                    {
                        Email = Email,
                        Password = Password
                    };

                    string redirectUrl = await _userService.LoginAsync(loginModel);

                    // Chuyển hướng đến đường dẫn nhận được
                    return Redirect(redirectUrl);
                }
                catch (Exception ex)
                {
                    ErrorMessage = ex.Message;
                    return Page();
                }
            }
            return Page();
        }

    }

}



