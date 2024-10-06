﻿using AutoMapper;
using ToyShop.Core.Base;
using ToyShop.Core.Store;
using ToyShop.Core.Utils;
using ToyShop.ModelViews.ContractModelView;
using Microsoft.EntityFrameworkCore;
using ToyShop.Contract.Repositories.Interface;
using ToyShop.Core.Constants;
using static ToyShop.Core.Base.BaseException;
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
            ToyShop.Contract.Repositories.Entity.ContractEntity newContract = _mapper.Map<ToyShop.Contract.Repositories.Entity.ContractEntity>(model);
            newContract.CreatedTime = CoreHelper.SystemTimeNow;

            await _unitOfWork.GetRepository<ToyShop.Contract.Repositories.Entity.ContractEntity>().InsertAsync(newContract);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteContractAsync(string id)
        {
            // Lấy sản phẩm - kiểm tra sự tồn tại

            ToyShop.Contract.Repositories.Entity.ContractEntity contract = await _unitOfWork.GetRepository<ToyShop.Contract.Repositories.Entity.ContractEntity>().Entities
                .FirstOrDefaultAsync(p => p.Id == id && !p.DeletedTime.HasValue)
                ?? throw new ErrorException((int)StatusCodeHelper.Notfound, ResponseCodeConstants.NOT_FOUND, "Contract not found!");

            // Xóa mềm
            contract.DeletedTime = CoreHelper.SystemTimeNow;

            _unitOfWork.GetRepository<ToyShop.Contract.Repositories.Entity.ContractEntity>().Update(contract);
            await _unitOfWork.SaveAsync();
        }


        public async Task<BasePaginatedList<ToyShop.Contract.Repositories.Entity.ContractEntity>> GetContractsAsync(int pageNumber, int pageSize)
        {
            // pagenumber >= 1, min 1
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            // pageSize >= 1, min 5
            pageSize = pageSize < 1 ? 5 : pageSize;

            // contract chua bi xoa
            IQueryable<ToyShop.Contract.Repositories.Entity.ContractEntity> contractsQuery = _unitOfWork.GetRepository<ToyShop.Contract.Repositories.Entity.ContractEntity>().Entities
                .Where(p => !p.DeletedTime.HasValue)
                .OrderByDescending(p => p.CreatedTime);

            int totalCount = await contractsQuery.CountAsync();

            // Apply pagination
            List<ToyShop.Contract.Repositories.Entity.ContractEntity> paginatedProducts = await contractsQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();


            return new BasePaginatedList<ToyShop.Contract.Repositories.Entity.ContractEntity>(paginatedProducts, totalCount, pageNumber, pageSize);
        }

        public async Task<ResponseContractModel> GetContractAsync(string id)
        {
            ToyShop.Contract.Repositories.Entity.ContractEntity contract = await _unitOfWork.GetRepository<ToyShop.Contract.Repositories.Entity.ContractEntity>().Entities
                                                                    .FirstOrDefaultAsync(p => p.Id == id && !p.DeletedTime.HasValue) ?? 
                                                                    throw new ErrorException((int)StatusCodeHelper.Notfound, ResponseCodeConstants.NOT_FOUND, "Contract not found!");
            return _mapper.Map<ResponseContractModel>(contract);

        }


        public async Task UpdateContractAsync(string id, UpdateContractModel model)
        {
            
            ToyShop.Contract.Repositories.Entity.ContractEntity contract = await _unitOfWork.GetRepository<ToyShop.Contract.Repositories.Entity.ContractEntity>().Entities
                .FirstOrDefaultAsync(p => p.Id == id && !p.DeletedTime.HasValue)
                ?? throw new ErrorException((int)StatusCodeHelper.Notfound, ResponseCodeConstants.NOT_FOUND, "Contract not found!");
            
            _mapper.Map(model, contract);
            contract.LastUpdatedTime = CoreHelper.SystemTimeNow;

            _unitOfWork.GetRepository<ToyShop.Contract.Repositories.Entity.ContractEntity>().Update(contract);
            await _unitOfWork.SaveAsync();

        }
    }
}
