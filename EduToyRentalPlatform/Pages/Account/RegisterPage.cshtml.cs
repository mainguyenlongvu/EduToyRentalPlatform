using ToyShop.Contract.Repositories.Entity;
using ToyShop.Contract.Services.Interface;
using ToyShop.ModelViews.UserModelViews;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using ToyShop.Repositories.Base;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Http;



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
        [EmailAddress]
        [Required(ErrorMessage = "Hãy nhập Email")]
        public string Email { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Hãy nhập mật khẩu.")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
        public string Password { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Hãy nhập tên tài khoản.")]
        public string UserName { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Hãy nhập số điện thoại.")]
        public string Phone { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Hãy chọn vai trò.")]
        public string RoleId { get; set; }

        [BindProperty]
        public List<SelectListItem> Quyen { get; set; }

        public string ErrorMessage { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var registerModel = new RegisterModel
            {
                UserName = UserName,
                Email = Email,
                Password = Password,
                Phone = Phone,
                RoleName = "Customer" // Lấy RoleId của quyền "user"
            };

            try
            {
                // Gọi hàm RegisterAsync
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
