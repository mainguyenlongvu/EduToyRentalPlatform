using EduToyRentalPlatform.SignalR;
using System.Runtime.CompilerServices;
using ToyShop;
using ToyShop.Middleware;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddRazorPages();


builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddRazorPages();
builder.Services.AddConfig(builder.Configuration);
DependencyInjection.AddServices(builder.Services, builder.Configuration);

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseLogoutMiddleware();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseSession();
app.MapRazorPages();

// Định nghĩa route cho trang chủ
app.MapGet("/", async context =>
{
    // Kiểm tra quyền người dùng
    bool UserHasAccess()
    {
        var role = context.Request.Cookies["UserRole"];
        return role == "Admin";
    }

    if (UserHasAccess())
    {
        context.Response.Redirect("/Admin/Index");
    }
    else
    {
        context.Response.Redirect("/Shop");
    }

    await Task.CompletedTask;
});

app.MapHub<NotificationHub>("notification-hub");
app.MapHub<MessageHub>("message-hub");

app.Run();