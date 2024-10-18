using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ToyShop.Pages.Admin
{
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            //// Ki?m tra quy?n truy c?p
            //if (!UserHasAccess())
            //{
            //    // N?u không có quy?n, chuy?n h??ng v? trang khác
            //    return RedirectToPage("/Account/ErrorPage");
            //}

            return Page();
        }

        private bool UserHasAccess()
        {
            var role = HttpContext.Request.Cookies["UserRole"];
            return role == "Admin"; // Ki?m tra xem có ph?i là Admin không
        }
    }
}
