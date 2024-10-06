using ToyShop.Contract.Repositories.Entity;
using ToyShop.Core.Base;
using ToyShop.ModelViews.ToyModelViews;

namespace ToyShop.Contract.Services.Interface
{
    public interface IToyService
    {
        Task<ResponeToyModel> GetToyAsync(string id);
        Task<BasePaginatedList<ResponeToyModel>> GetToysAsync(int pageNumber, int pageSize, bool? sortByName);
        Task<bool> CreateToyAsync(CreateToyModel model);
        Task<bool> UpdateToyAsync(string id, UpdateToyModel model);
        Task<bool> DeleteToyAsync(string id);
    }
}
