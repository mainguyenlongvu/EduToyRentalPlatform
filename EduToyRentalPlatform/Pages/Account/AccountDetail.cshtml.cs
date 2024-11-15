using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToyShop.ModelViews.UserModelViews;
using ToyShop.Repositories.Base;
using ToyShop.Repositories.Entity;
using ToyShop.Contract.Repositories.Entity;
using ToyShop.Contract.Services.Interface;
using System.Drawing;
using ToyShop.Core.Utils;

namespace ToyShop.Pages.Account
{
    public class AccountDetailModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountDetailModel(IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }
        [BindProperty]
        public string UserName { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Phone { get; set; }
        [BindProperty]
        public string Img { get; set; }
        [BindProperty]
        public string FullName { get; set; }
        [BindProperty]
        public string NewPassword { get; set; }

        [BindProperty]
        public string ConfirmPassword { get; set; }
        [BindProperty]
        public IFormFile ImageFile { get; set; }

        [BindProperty]
        public UpdateCustomerModel UserDetails { get; set; } = new UpdateCustomerModel();

        [BindProperty]
        public ChangPasswordModel ChangePassword { get; set; } = new ChangPasswordModel();

        public async Task<IActionResult> OnGetAsync()
        {
            string userId = _httpContextAccessor.HttpContext?.Request.Cookies["UserId"];
            // Load the user details
            var user = await _userService.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }
            // Gán giá trị vào UserDetails
            UserDetails.FullName = user.FullName;
            UserDetails.Phone = user.Phone;
            UserDetails.ImageUrl = user.ImageUrl;

            //Gán giá trị 
            Phone = user.Phone;
            Email = user.Email;
            UserName = user.UserName;
            FullName = user.FullName;
            Img = user.ImageUrl;
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateDetailAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string userId = _httpContextAccessor.HttpContext?.Request.Cookies["UserId"];
            if (string.IsNullOrEmpty(userId))
            {
                // Xử lý khi không có UserId (có thể là redirect về trang đăng nhập)
                return RedirectToPage("/Account/Login");
            }
            if (ImageFile != null)
            {
                UserDetails.ImageUrl = await FileUploadHelper.UploadFile(ImageFile);
            }

            await _userService.UpdateCustomerAsync(Guid.Parse(userId), UserDetails);

            return RedirectToPage("/Account/AccountDetail");
        }


        public async Task<IActionResult> OnPostChangePasswordAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            string userId = _httpContextAccessor.HttpContext?.Request.Cookies["UserId"];
            // Call ChangePasswordAsync to change the password
            await _userService.ChangPasswordAsync(ChangePassword);

            return RedirectToPage("/Account/AccountDetail");
        }
    }
}
