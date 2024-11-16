using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using ToyShop.Contract.Services.Interface;
using ToyShop.ModelViews.UserModelViews;

namespace EduToyRentalPlatform.Pages.Account
{
    public class ForgetPasswordModel : PageModel
    {
        private readonly IUserService _userService;

        public ForgetPasswordModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        [Required(ErrorMessage = "Hãy nhập Email.")]
        public string Email { get; set; }

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

            try
            {
                // Call RegisterAsync to save the registration data, including the image
                await _userService.ForgotPassword(Email);


                ErrorMessage = "Gửi mật khẩu thành công! Vui lòng kiểm tra email.";
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
