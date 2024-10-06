using ToyShop.ModelViews.UserModelViews;
namespace ToyShop.Contract.Services.Interface
{
    public interface IUserService
    {
        Task<string> LoginAsync(LoginModel model);
        Task<bool> RegisterAsync(RegisterModel model);
    }
}
