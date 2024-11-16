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
        public int Money { get; set; }
        [BindProperty]
        public string NewPassword { get; set; }
        [BindProperty]
        public int ToUpMoney { get; set; }

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
            Money = user.Money;
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateDetailAsync()
        {

            // Lấy UserId từ cookie
            string userId = _httpContextAccessor.HttpContext?.Request.Cookies["UserId"];
            if (string.IsNullOrEmpty(userId))
            {
                // Chuyển hướng về trang đăng nhập nếu UserId không tồn tại
                return RedirectToPage("/Account/Login");
            }

            // Xử lý tải lên ảnh (nếu có)
            if (ImageFile != null)
            {
                // Kiểm tra tệp hợp lệ
                if (ImageFile.Length > 0 && (ImageFile.ContentType.Contains("image/jpeg") || ImageFile.ContentType.Contains("image/png")))
                {
                    // Upload ảnh và cập nhật URL
                    UserDetails.ImageUrl = await FileUploadHelper.UploadFile(ImageFile);
                }
                else
                {
                    // Thêm lỗi nếu tệp không hợp lệ
                    ModelState.AddModelError("ImageFile", "Tệp phải là hình ảnh định dạng JPEG hoặc PNG.");
                    return RedirectToPage("/Account/AccountDetail");
                }
            }

            // Gửi yêu cầu cập nhật người dùng
            try
            {
                await _userService.UpdateCustomerAsync(Guid.Parse(userId), UserDetails);
            }
            catch (Exception ex)
            {
                // Log lỗi và hiển thị thông báo
                ModelState.AddModelError(string.Empty, $"Đã xảy ra lỗi: {ex.Message}");
                return RedirectToPage("/Account/AccountDetail");
            }

            // Chuyển hướng về trang chi tiết tài khoản sau khi cập nhật thành công
            return RedirectToPage("/Account/AccountDetail");
        }



        public async Task<IActionResult> OnPostToUpMoneyAsync()
        {
            string userId = _httpContextAccessor.HttpContext?.Request.Cookies["UserId"];
            // Call ChangePasswordAsync to change the password
            await _userService.ChangPasswordAsync(ChangePassword);

            return RedirectToPage("/Account/AccountDetail");
        }
    }
}
