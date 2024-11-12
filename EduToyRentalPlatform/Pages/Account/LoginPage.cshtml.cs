using ToyShop.Contract.Repositories.Entity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using ToyShop.Contract.Services.Interface;
using ToyShop.ModelViews.UserModelViews;
using ToyShop.Contract.Repositories.Interface;


namespace ToyShop.Pages.Account
{
    public class LoginPageModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        public LoginPageModel(IUserService userService, IUnitOfWork unitOfWork)
        {
            _userService = userService;
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        [Required(ErrorMessage = "Hãy nhập Email tài khoản.")]
        //[EmailAddress(ErrorMessage = "Email không hợp lệ.")]
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


            //if (ModelState.IsValid)
            //{
            try
            {
                var loginModel = new LoginModel
                {
                    Email = Email,
                    Password = Password
                };

                string redirectUrl = await _userService.LoginAsync(loginModel);
                var user = await _userService.GetUserAsync(loginModel);
                Response.Cookies.Append("UserName", Email, new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(7),
                    HttpOnly = true
                });
                Response.Cookies.Append("UserId", user.Id.ToString(), new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(7),
                    HttpOnly = true
                });
                ErrorMessage = "Đăng nhập thành công!";
                // Chuyển hướng đến đường dẫn nhận được
                return Redirect(redirectUrl);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            //}

            return Page();
        }
    }
}
