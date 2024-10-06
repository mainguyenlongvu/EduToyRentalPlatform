using ToyShop.Core.Base;
using ToyShop.ModelViews.ContractModelView;

namespace ToyShop.Contract.Services.Interface
{
    public interface IContractService
    {
        Task<ResponseContractModel> GetContractAsync(string id);
        Task<BasePaginatedList<ToyShop.Contract.Repositories.Entity.ContractEntity>> GetContractsAsync(int pageNumber, int pageSize);
        Task CreateContractAsync(CreateContractModel model);
        Task UpdateContractAsync(string id, UpdateContractModel model);
        Task DeleteContractAsync(string id);


    }
}
