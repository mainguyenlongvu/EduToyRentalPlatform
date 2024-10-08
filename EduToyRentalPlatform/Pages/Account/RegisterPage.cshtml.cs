using ToyShop.Contract.Repositories.Entity;
using ToyShop.Contract.Services.Interface;
using ToyShop.ModelViews.UserModelViews;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using ToyShop.Repositories.Base;

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
        [Required(ErrorMessage = "Hãy nhập Email")]
        public string Email { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Hãy nhập Password")]
        public string Password { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Hãy nhập UserName")]
        public string UserName { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Hãy nhập Phone")]
        public string Phone { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Hãy chọn vai trò.")]
        public string RoleName { get; set; }

        [BindProperty]
        public List<SelectListItem> Quyen { get; set; }

        public void OnGet()
        {
            //Quyen = _context.Roles.Select(r => new SelectListItem
            //{
            //    /*Value = r.Id,*/ // Nếu bạn có Id để lưu vào RoleId
            //    Text = r.Name
            //}).ToList();

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
                RoleName = RoleName
            };

            try
            {
                // Gọi hàm RegisterAsync
                bool result = await _userService.RegisterAsync(registerModel);

                if (result)
                {
                    return RedirectToPage("/Account/Login");
                }
            }
            catch (Exception ex)
            {
                // Ghi log hoặc xử lý lỗi ở đây
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return Page();
        }

    }

}

