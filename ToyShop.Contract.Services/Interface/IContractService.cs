﻿using System.Threading.Tasks;
using ToyShop.Contract.Repositories.Entity;
using ToyShop.Core.Base;
using ToyShop.ModelViews.ContractModelView;
using ToyShop.ModelViews.TopUpModel;

namespace ToyShop.Contract.Services.Interface
{
    public interface IContractService
    {
        Task<ResponseContractModel> GetContractAsync(string id);
        Task<BasePaginatedList<ContractEntity>> GetContractsAsync(int pageNumber, int pageSize, string searchTerm = null);
        Task CreateContractAsync(CreateContractModel model);
        Task UpdateContractAsync(string id, UpdateContractModel model);
        Task DeleteContractAsync(string id);
        Task<List<ContractEntity>> GetAllContractsAsync(); // New method for fetching all contracts
        Task PayByWalletAsync(string id, PayByWalletModel model);
        Task DirectPaymentAsync(string id, DirectPaymentModel model);
        Task<bool> CreateTopUpAsync(CreateTopUpModel model);
    }
}
