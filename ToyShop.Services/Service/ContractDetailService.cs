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
using System.Diagnostics.Contracts;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using ToyShop.Repositories.Entity;
using Microsoft.AspNet.Identity;
using ToyShop.Contract.Repositories.PaggingItems;
namespace ToyShop.Contract.Services.Interface
{
    public class ContractDetailService : IContractDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ContractDetailService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task CreateContractDetailAsync(CreateContractDetailModel model)
        {
            Toy toy = await _unitOfWork.GetRepository<Toy>().Entities.FirstOrDefaultAsync(x => x.Id == model.ToyId && !x.DeletedTime.HasValue) ?? throw new ErrorException((int)StatusCodeHelper.Notfound, ResponseCodeConstants.INVALID_INPUT, "Phải nhập mã đồ chơi");
            if (model.Quantity <= 0)
            {
                throw new ErrorException((int)StatusCodeHelper.Notfound, ResponseCodeConstants.INVALID_INPUT, "Số lượng phải lớn hơn 0");
            }
            //Lấy Id của user
            string userId = _httpContextAccessor.HttpContext?.Request.Cookies["UserId"];
            //Tìm đơn hàng có trạng thái là In Cart
            ContractEntity contractEntity = await _unitOfWork.GetRepository<ContractEntity>().Entities.FirstOrDefaultAsync(x => x.UserId.ToString() == userId && x.Status == "In Cart" && !x.DeletedTime.HasValue);
            //Nếu không tìm thấy thì tạo mới 
            if (contractEntity == null)
            {
                ContractEntity newContract = new ContractEntity();
                newContract.Status = "In Cart";
                newContract.CreatedBy = userId;
                newContract.CreatedTime = CoreHelper.SystemTimeNows;
                //Map qua 
                ContractDetail contractDetail = _mapper.Map<ContractDetail>(model);
                contractDetail.CreatedTime = CoreHelper.SystemTimeNows;
                contractDetail.CreatedBy = userId;
                contractDetail.ContractId = newContract.Id;
                contractDetail.Price = model.Quantity * toy.ToyPrice;
                await _unitOfWork.GetRepository<ContractDetail>().InsertAsync(contractDetail);
                await _unitOfWork.SaveAsync();
            }
            else
            {
                ContractDetail newContractDetail = _mapper.Map<ContractDetail>(model);
                newContractDetail.CreatedTime = CoreHelper.SystemTimeNows;
                newContractDetail.CreatedBy = userId;
                newContractDetail.ContractId = contractEntity.Id;
                newContractDetail.Price = model.Quantity * toy.ToyPrice;
                await _unitOfWork.GetRepository<ContractDetail>().InsertAsync(newContractDetail);
                await _unitOfWork.SaveAsync();
            }
        }

        public async Task DeleteContractDetailAsync(string id)
        {
            // Lấy sản phẩm - kiểm tra sự tồn tại

            ContractEntity contractDetail = await _unitOfWork.GetRepository<ContractEntity>().Entities
                .FirstOrDefaultAsync(p => p.Id == id && !p.DeletedTime.HasValue)
                ?? throw new ErrorException((int)StatusCodeHelper.Notfound, ResponseCodeConstants.NOT_FOUND, "Contract detail not found!");

            // Xóa mềm
            contractDetail.DeletedTime = CoreHelper.SystemTimeNows;

            _unitOfWork.GetRepository<ContractEntity>().Update(contractDetail);
            await _unitOfWork.SaveAsync();
        }


        public async Task<BasePaginatedList<ResponseContractDetailModel>> GetContractDetailsAsync(int pageNumber, int pageSize)
        {
            // pagenumber >= 1, min 1
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            // pageSize >= 1, min 5
            pageSize = pageSize < 1 ? 5 : pageSize;

            // contract chưa bị xóa
            IQueryable<ContractDetail> contractDetailsQuery = _unitOfWork.GetRepository<ContractDetail>().Entities
                .Where(p => !p.DeletedTime.HasValue)
                .Include(x=>x.Toy)
                .OrderByDescending(p => p.CreatedTime);

            int totalCount = contractDetailsQuery.Count();

            // Lấy dữ liệu phân trang từ repository
            BasePaginatedList<ContractDetail> paginatedContractDetail = await _unitOfWork.GetRepository<ContractDetail>()
                .GetPagging(contractDetailsQuery, pageNumber, pageSize);

            // Map qua ResponseContractDetailModel
            List<ResponseContractDetailModel> responseItems = paginatedContractDetail.Items.Select(item =>
            {
                return new ResponseContractDetailModel()
                {
                    ContractId = item.ContractId,
                    Id = item.Id,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    ToyId = item.ToyId,
                    ToyName = item.Toy.ToyName
                };
            }).ToList();

            // Trả về danh sách phân trang
            return new BasePaginatedList<ResponseContractDetailModel>(responseItems, totalCount, pageNumber, pageSize);
        }

        public async Task<ResponseContractDetailModel> GetContractDetailAsync(string id)
        {
            ContractEntity contractDetail = await _unitOfWork.GetRepository<ContractEntity>().Entities
                                                                    .FirstOrDefaultAsync(p => p.Id == id && !p.DeletedTime.HasValue) ??
                                                                    throw new ErrorException((int)StatusCodeHelper.Notfound, ResponseCodeConstants.NOT_FOUND, "Contract detail not found!");
            return _mapper.Map<ResponseContractDetailModel>(contractDetail);

        }

        public async Task UpdateContractDetailAsync(string id, UpdateContractDetailModel model)
        {
            //Điều kiện
            if (model.Quantity <= 0)
            {
                throw new ErrorException((int)StatusCodeHelper.Notfound, ResponseCodeConstants.INVALID_INPUT, "Số lượng phải lớn hơn 0");
            }
            //Tìm hợp đồng
            ContractDetail contractDetail = await _unitOfWork.GetRepository<ContractDetail>().Entities
                .FirstOrDefaultAsync(p => p.Id == id && !p.DeletedTime.HasValue)
                ?? throw new ErrorException((int)StatusCodeHelper.Notfound, ResponseCodeConstants.NOT_FOUND, "Contract detail not found!");
            //Tìm đồ chơi
            Toy toy = await _unitOfWork.GetRepository<Toy>().Entities
                .FirstOrDefaultAsync(p => p.Id == contractDetail.ToyId && !p.DeletedTime.HasValue);
            _mapper.Map(model, contractDetail);
            contractDetail.LastUpdatedTime = CoreHelper.SystemTimeNows;
            //Cập nhập lại giá
            contractDetail.Price = contractDetail.Quantity * toy.ToyPrice;
            _unitOfWork.GetRepository<ContractDetail>().Update(contractDetail);
            await _unitOfWork.SaveAsync();
        }
    }
}
