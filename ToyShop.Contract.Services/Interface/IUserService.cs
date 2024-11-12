using Microsoft.AspNet.Identity;
using ToyShop.Core.Base;
using ToyShop.ModelViews.UserModelViews;
using ToyShop.Repositories.Entity;
namespace ToyShop.Contract.Services.Interface
{
    public interface IUserService
    {
        Task<ApplicationUser> GetUserByIdAsync(string id);
        Task<string> LoginAsync(LoginModel model);
        Task<bool> RegisterAsync(RegisterModel model);
        Task<ApplicationUser> GetUserAsync(LoginModel model);
        Task<bool> DeleteUserAsync(string id);
        Task UpdateCustomerAsync(Guid id, UpdateCustomerModel model);
        Task ChangPasswordAdminAsync(ChangPasswordAdminModel model);
        Task<BasePaginatedList<ApplicationUser>> GetPageAsync(int index, int pageSize, string nameSearch);
        Task ChangPasswordAsync(ChangPasswordModel model);
    }
}
