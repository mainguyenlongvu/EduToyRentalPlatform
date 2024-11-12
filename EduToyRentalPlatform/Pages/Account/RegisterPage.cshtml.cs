using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using ToyShop.Contract.Services.Interface;
using ToyShop.ModelViews.UserModelViews;

namespace ToyShop.Pages.Account
{
    public class RegisterPageModel : PageModel
    {
        private readonly IUserService _userService;

        public RegisterPageModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        [Required(ErrorMessage = "Hãy nhập tên tài khoản.")]
        public string UserName { get; set; }

        [BindProperty]
        [EmailAddress]
        [Required(ErrorMessage = "Hãy nhập Email")]
        public string Email { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Hãy nhập mật khẩu.")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
        public string Password { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Hãy nhập số điện thoại.")]
        public string Phone { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Hãy chọn vai trò.")]
        public string RoleId { get; set; }

        [BindProperty]
        public List<SelectListItem> Quyen { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Hãy chọn ảnh đại diện.")]
        public IFormFile Image { get; set; } // Bind the image file

        public string ErrorMessage { get; set; }

        public void OnGet()
        {
            // Load roles or other data for the page as necessary
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var registerModel = new RegisterModel
            {
                UserName = UserName,
                Email = Email,
                Password = Password,
                Phone = Phone,
                RoleName = "Customer", // Set the role name; adjust as needed
                Image = Image // Assign the uploaded file to the model
            };

            try
            {
                // Call RegisterAsync to save the registration data, including the image
                bool result = await _userService.RegisterAsync(registerModel);

                if (result)
                {
                    ErrorMessage = "Đăng ký thành công! Vui lòng đăng nhập.";
                    return RedirectToPage("/Account/LoginPage");
                }
                else
                {
                    ErrorMessage = "Đăng ký không thành công! Vui lòng thử lại.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }

            return Page();
        }
    }
}
