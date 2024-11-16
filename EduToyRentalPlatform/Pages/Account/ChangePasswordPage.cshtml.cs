using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using ToyShop.Contract.Services.Interface;
using ToyShop.ModelViews.UserModelViews;

namespace EduToyRentalPlatform.Pages.Account
{
    public class ChangePasswordPageModel : PageModel
    {
        private readonly IUserService _userService;

        public ChangePasswordPageModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        [Required(ErrorMessage = "Hãy nhập tên tài khoản.")]
        public string UserName { get; set; }

        [BindProperty]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
        [Required(ErrorMessage = "Hãy nhập mật khẩu mới")]
        public string NewPassword { get; set; }
        [BindProperty]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
        [Required(ErrorMessage = "Hãy nhập lại mật khẩu mới")]
        public string ConfirmPassword { get; set; }

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
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var changPassword = new ChangPasswordModel
            {
                UserName = UserName,
                NewPassword = NewPassword,
                Password = Password,
                ConfirmPassword = ConfirmPassword,
            };

            try
            {
                // Call RegisterAsync to save the registration data, including the image
                await _userService.ChangPasswordAsync(changPassword);


                ErrorMessage = "Đổi mật khẩu thành công! Vui lòng đăng nhập.";
                return RedirectToPage("/Account/LoginPage");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }

            return Page();
        }
    }
}
