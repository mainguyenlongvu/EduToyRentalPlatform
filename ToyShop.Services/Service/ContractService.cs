using AutoMapper;
using ToyShop.Core.Base;
using ToyShop.Core.Store;
using ToyShop.Core.Utils;
using ToyShop.ModelViews.ContractModelView;
using Microsoft.EntityFrameworkCore;
using ToyShop.Contract.Repositories.Interface;
using ToyShop.Core.Constants;
using static ToyShop.Core.Base.BaseException;
using ToyShop.Contract.Repositories.Entity;

namespace ToyShop.Contract.Services.Interface
{
    public class ContractService : IContractService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ContractService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateContractAsync(CreateContractModel model)
        {
            model.CheckValidate();
            ContractEntity newContract = _mapper.Map<ContractEntity>(model);
            newContract.CreatedTime = CoreHelper.SystemTimeNows;

            await _unitOfWork.GetRepository<ContractEntity>().InsertAsync(newContract);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteContractAsync(string id)
        {
            ContractEntity contract = await _unitOfWork.GetRepository<ContractEntity>().Entities
                .FirstOrDefaultAsync(p => p.Id == id && !p.DeletedTime.HasValue)
                ?? throw new ErrorException((int)StatusCodeHelper.Notfound, ResponseCodeConstants.NOT_FOUND, "Contract not found!");

            contract.DeletedTime = CoreHelper.SystemTimeNows;

            _unitOfWork.GetRepository<ContractEntity>().Update(contract);
            await _unitOfWork.SaveAsync();
        }

        public async Task<BasePaginatedList<ContractEntity>> GetContractsAsync(int pageNumber, int pageSize)
        {
            // Validate page number and page size
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 5 : pageSize;

            // Query for non-deleted contracts
            IQueryable<ContractEntity> contractsQuery = _unitOfWork.GetRepository<ContractEntity>().Entities
                .Where(p => !p.DeletedTime.HasValue)
                .OrderByDescending(p => p.CreatedTime);

            int totalCount = await contractsQuery.CountAsync();

            // Apply pagination
            List<ContractEntity> paginatedContracts = await contractsQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new BasePaginatedList<ContractEntity>(paginatedContracts, totalCount, pageNumber, pageSize);
        }

        public async Task<ResponseContractModel> GetContractAsync(string id)
        {
            ContractEntity contract = await _unitOfWork.GetRepository<ContractEntity>().Entities
                .FirstOrDefaultAsync(p => p.Id == id && !p.DeletedTime.HasValue)
                ?? throw new ErrorException((int)StatusCodeHelper.Notfound, ResponseCodeConstants.NOT_FOUND, "Contract not found!");

            return _mapper.Map<ResponseContractModel>(contract);
        }

        public async Task UpdateContractAsync(string id, UpdateContractModel model)
        {
            ContractEntity contract = await _unitOfWork.GetRepository<ContractEntity>().Entities
                .FirstOrDefaultAsync(p => p.Id == id && !p.DeletedTime.HasValue)
                ?? throw new ErrorException((int)StatusCodeHelper.Notfound, ResponseCodeConstants.NOT_FOUND, "Contract not found!");

            if (model == null)
                throw new ArgumentNullException(nameof(model), "Update model cannot be null.");

            _mapper.Map(model, contract);
            contract.LastUpdatedTime = CoreHelper.SystemTimeNows;

            _unitOfWork.GetRepository<ContractEntity>().Update(contract);
            await _unitOfWork.SaveAsync();
        }

        // Fetching all contracts without soft delete filtering (consider if you need this)
        public async Task<List<ContractEntity>> GetAllContractsAsync()
        {
            return await _unitOfWork.GetRepository<ContractEntity>().Entities.ToListAsync(); 
        }
    }
}