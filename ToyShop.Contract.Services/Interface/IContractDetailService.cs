using ToyShop.Contract.Repositories.Entity;
using ToyShop.Core.Base;
using ToyShop.ModelViews.ContractDetailModelView;
using ToyShop.ModelViews.ContractModelView;

namespace ToyShop.Contract.Services.Interface
{
    public interface IContractDetailService
    {
        Task<ResponseContractDetailModel> GetContractDetailAsync(string id);
        Task<BasePaginatedList<ResponseContractDetailModel>> GetContractDetailsAsync(int pageNumber, int pageSize);
        Task CreateContractDetailAsync(CreateContractDetailModel model);
        Task UpdateContractDetailAsync(string id, UpdateContractDetailModel model);
        Task DeleteContractDetailAsync(string id);


    }
}
