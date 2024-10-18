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
using ToyShop.ModelViews.ContractDetailModelView;
namespace ToyShop.Contract.Services.Interface
{
    public class ContractDetailService : IContractDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ContractDetailService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateContractDetailAsync(CreateContractDetailModel model)
        {
            model.CheckValidate();
            ContractDetail newContractDetail = _mapper.Map<ContractDetail>(model);
            newContractDetail.CreatedTime = CoreHelper.SystemTimeNows;

            await _unitOfWork.GetRepository<ContractDetail>().InsertAsync(newContractDetail);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteContractDetailAsync(string id)
        {
            // Lấy sản phẩm - kiểm tra sự tồn tại

            ContractEntity contractDetail = await _unitOfWork.GetRepository<ContractEntity>().Entities
                .FirstOrDefaultAsync(p => p.Id == id && !p.DeletedTime.HasValue)
                ?? throw new ErrorException((int)StatusCodeHelper.Notfound, ResponseCodeConstants.NOT_FOUND, "Contract not found!");

            // Xóa mềm
            contractDetail.DeletedTime = CoreHelper.SystemTimeNows;

            _unitOfWork.GetRepository<ContractEntity>().Update(contractDetail);
            await _unitOfWork.SaveAsync();
        }


        public async Task<BasePaginatedList<ContractDetail>> GetContractDetailsAsync(int pageNumber, int pageSize)
        {
            // pagenumber >= 1, min 1
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            // pageSize >= 1, min 5
            pageSize = pageSize < 1 ? 5 : pageSize;

            // contract chua bi xoa
            IQueryable<ContractDetail> contractDetailsQuery = _unitOfWork.GetRepository<ContractDetail>().Entities
                .Where(p => !p.DeletedTime.HasValue)
                .OrderByDescending(p => p.CreatedTime);

            int totalCount = await contractDetailsQuery.CountAsync();

            // Apply pagination
            List<ContractDetail> paginatedProducts = await contractDetailsQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();


            return new BasePaginatedList<ContractDetail>(paginatedProducts, totalCount, pageNumber, pageSize);
        }

        public async Task<ResponseContractDetailModel> GetContractDetailAsync(string id)
        {
            ContractEntity contractDetail = await _unitOfWork.GetRepository<ContractEntity>().Entities
                                                                    .FirstOrDefaultAsync(p => p.Id == id && !p.DeletedTime.HasValue) ?? 
                                                                    throw new ErrorException((int)StatusCodeHelper.Notfound, ResponseCodeConstants.NOT_FOUND, "Contract not found!");
            return _mapper.Map<ResponseContractDetailModel>(contractDetail);

        }


        public async Task UpdateContractDetailAsync(string id, UpdateContractDetailModel model)
        {

            ContractDetail contractDetail = await _unitOfWork.GetRepository<ContractDetail>().Entities
                .FirstOrDefaultAsync(p => p.Id == id && !p.DeletedTime.HasValue)
                ?? throw new ErrorException((int)StatusCodeHelper.Notfound, ResponseCodeConstants.NOT_FOUND, "Contract not found!");
            
            _mapper.Map(model, contractDetail);
            contractDetail.LastUpdatedTime = CoreHelper.SystemTimeNows;

            _unitOfWork.GetRepository<ContractDetail>().Update(contractDetail);
            await _unitOfWork.SaveAsync();

        }
    }
}
