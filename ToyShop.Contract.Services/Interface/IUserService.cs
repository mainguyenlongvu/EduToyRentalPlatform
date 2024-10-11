using Microsoft.AspNet.Identity;
using ToyShop.ModelViews.UserModelViews;
using ToyShop.Repositories.Entity;
namespace ToyShop.Contract.Services.Interface
{
    public interface IUserService
    {
        Task<ApplicationUser> GetUserByIdAsync(string id);
        Task<string> LoginAsync(LoginModel model);
        Task<bool> RegisterAsync(RegisterModel model);
    }
}
