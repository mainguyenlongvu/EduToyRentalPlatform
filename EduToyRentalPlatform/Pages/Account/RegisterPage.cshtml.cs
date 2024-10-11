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
        private readonly ToyShopDBContext _context;
        private readonly IUserService _userService;

        public RegisterPageModel(ToyShopDBContext context, IUserService userService)
        {
            _context = context;
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

        public void OnGet()
        {
            Quyen = _context.ApplicationRoles.Select(r => new SelectListItem
            {
                Value = r.Id.ToString(),
                Text = r.Name
            }).ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var registerModel = new RegisterModel
                {
                    UserName = UserName,
                    Email = Email,
                    Password = Password,
                    Phone = Phone,
                    RoleId = RoleId
                };

                try
                {
                    // Gọi hàm RegisterAsync
                    bool result = await _userService.RegisterAsync(registerModel);

                    if (result)
                    {
                        TempData["SuccessMessage"] = "Đăng ký thành công! Vui lòng đăng nhập.";
                        return RedirectToPage("/Account/LoginPage");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Đăng ký không thành công. Vui lòng thử lại.");
                    }
                }
                catch (Exception ex)
                {
                    // Ghi log hoặc xử lý lỗi ở đây
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            // Nếu có lỗi trong ModelState hoặc đăng ký không thành công, trả về trang hiện tại
            return Page();
        }

    }
}
