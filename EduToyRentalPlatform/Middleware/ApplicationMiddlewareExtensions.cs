using Microsoft.AspNetCore.Builder;

namespace ToyShop.Middleware
{
    public static class ApplicationMiddlewareExtensions
    {
        public static void UseLogoutMiddleware(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                // Nếu người dùng truy cập đường dẫn logout
                if (context.Request.Path == "/Account/LogoutPage")
                {
                    // Xóa các cookie lưu thông tin người dùng
                    context.Response.Cookies.Delete("UserName");
                    context.Response.Cookies.Delete("UserId");
                    context.Response.Cookies.Delete("UserRole");

                    // Chuyển hướng về trang login sau khi logout
                    context.Response.Redirect("/Account/LoginPage");
                }
                else
                {
                    // Nếu không phải trang logout, tiếp tục request bình thường
                    await next.Invoke();
                }
            });
        }
    }
}
