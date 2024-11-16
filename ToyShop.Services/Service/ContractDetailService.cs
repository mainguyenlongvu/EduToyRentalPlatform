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

        //public async Task CreateContractDetailAsync(CreateContractDetailModel model)
        //{
        //    Toy toy = await _unitOfWork.GetRepository<Toy>().Entities.FirstOrDefaultAsync(x => x.Id == model.ToyId && !x.DeletedTime.HasValue) ?? throw new ErrorException((int)StatusCodeHelper.Notfound, ResponseCodeConstants.INVALID_INPUT, "Phải nhập mã đồ chơi");
        //    if (model.Quantity <= 0)
        //    {
        //        throw new Exception("Số lượng phải lớn hơn 0");
        //    }
        //    if( model.DateStart >= model.DateEnd)
        //    {
        //        throw new Exception( "Ngày bắt đầu phải lớn hơn ngày kết thúc");
        //    }
        //    //Lấy Id của user
        //    string userId = _httpContextAccessor.HttpContext?.Request.Cookies["UserId"];
        //    //Tìm đơn hàng có trạng thái là In Cart
        //    ContractEntity contractEntity = await _unitOfWork.GetRepository<ContractEntity>().Entities.FirstOrDefaultAsync(x => x.UserId.ToString() == userId && x.Status == "In Cart" && !x.DeletedTime.HasValue);
        //    //Nếu không tìm thấy thì tạo mới 
        //    if (contractEntity == null)
        //    {
        //        // Tạo mới contract khi chưa có ở trạng thái In Cart
        //        ContractEntity newContract = new ContractEntity
        //        {
        //            Status = "In Cart",
        //            CreatedBy = userId,
        //            CreatedTime = CoreHelper.SystemTimeNows,
        //            UserId = Guid.Parse(userId)
        //        };
        //        // Lưu newContract vào cơ sở dữ liệu trước
        //        await _unitOfWork.GetRepository<ContractEntity>().InsertAsync(newContract);
        //        await _unitOfWork.SaveAsync();  // Sau khi save, newContract.Id sẽ có giá trị hợp lệ
        //        //Map qua 
        //        ContractDetail contractDetail = _mapper.Map<ContractDetail>(model);
        //        contractDetail.CreatedTime = CoreHelper.SystemTimeNows;
        //        contractDetail.CreatedBy = userId;
        //        contractDetail.ContractId = newContract.Id;
        //        //Nêu true là bán lấy giá bán
        //        if (model.ContractType == true)
        //        {
        //            contractDetail.Price = model.Quantity * toy.ToyPriceSale;
        //            newContract.TotalValue += contractDetail.Price;
        //        }
        //        else//còn false là thuê lấy giá của thuê
        //        {
        //            int datRent = (model.DateEnd - model.DateStart).Value.Days;
        //            contractDetail.Price = model.Quantity * (toy.ToyPriceRent + toy.ToyPriceSale / 2)* datRent;

        //            //Tạo đơn trả đồ chưa khi false luôn
        //            RestoreToy restoreToy = new RestoreToy();
        //            restoreToy.ContractId = newContract.Id;
        //            //Tạo đơn của từng sản phẩm
        //            RestoreToyDetail restoreToyDetail = new RestoreToyDetail();
        //            restoreToyDetail.RestoreToyId = restoreToy.Id;
        //            restoreToyDetail.ToyQuality = model.Quantity;
        //            restoreToyDetail.ToyName = toy.ToyName;
        //            restoreToyDetail.ToyId = toy.Id;
        //            restoreToyDetail.Reward = model.Quantity * toy.ToyPriceSale / 2;

        //            newContract.TotalValue += contractDetail.Price;
        //            //thêm vào Db
        //            await _unitOfWork.GetRepository<RestoreToy>().InsertAsync(restoreToy);
        //            await _unitOfWork.SaveAsync();
        //            await _unitOfWork.GetRepository<RestoreToyDetail>().InsertAsync(restoreToyDetail);
        //            await _unitOfWork.SaveAsync();
        //        }


        //        await _unitOfWork.GetRepository<ContractEntity>().UpdateAsync(newContract);
        //        //Lưu vào Db
        //        await _unitOfWork.GetRepository<ContractDetail>().InsertAsync(contractDetail);

        //        await _unitOfWork.SaveAsync();
        //    }
        //    else
        //    {
        //        //Tìm có trong DB ko
        //        ContractDetail contractDetail = await _unitOfWork.GetRepository<ContractDetail>().Entities.FirstOrDefaultAsync(x => x.ContractId == contractEntity.Id && !x.DeletedTime.HasValue && x.ToyId == model.ToyId && x.ContractType == model.ContractType);
        //        //Nếu đã có trong giỏ hàng
        //        if (contractDetail != null)
        //        {
        //            //Nêu true là bán lấy giá bán
        //            if (model.ContractType == true)
        //            {
        //                contractDetail.Price += model.Quantity * toy.ToyPriceSale;
        //                contractEntity.TotalValue += contractDetail.Price;
        //            }
        //            //còn false là thuê lấy giá của thuê
        //            else
        //            {
        //                int datRent = (model.DateEnd - model.DateStart).Value.Days;
        //                contractDetail.Price += model.Quantity * (toy.ToyPriceRent + toy.ToyPriceSale / 2) * datRent;
        //                contractEntity.TotalValue += contractDetail.Price;

        //                //Tìm xem có sẵn đơn trả chưa
        //                RestoreToy restoreToy = _unitOfWork.GetRepository<RestoreToy>().Entities.FirstOrDefault(x => x.ContractId == contractEntity.Id && !x.DeletedTime.HasValue);
        //                //Nếu có restoreToy
        //                if (restoreToy != null)
        //                {
        //                    //Tìm xem đơn trả đó có Đồ chơi này chưa
        //                    RestoreToyDetail restoreDetail = await _unitOfWork.GetRepository<RestoreToyDetail>().Entities.FirstOrDefaultAsync(x => x.RestoreToyId == restoreToy.Id && x.ToyId == toy.Id);
        //                    //Nếu có
        //                    if (restoreDetail != null)
        //                    {
        //                        restoreDetail.ToyQuality += model.Quantity;
        //                        restoreDetail.Reward += model.Quantity * toy.ToyPriceSale / 2;
        //                        //Cập nhật lại
        //                        await _unitOfWork.GetRepository<RestoreToyDetail>().UpdateAsync(restoreDetail);
        //                    }
        //                    //Nếu ko có
        //                    else
        //                    {
        //                        RestoreToyDetail restoreToyDetail = new RestoreToyDetail();
        //                        restoreToyDetail.RestoreToyId = restoreToy.Id;
        //                        restoreToyDetail.ToyQuality = model.Quantity;
        //                        restoreToyDetail.ToyName = toy.ToyName;
        //                        restoreToyDetail.ToyId = toy.Id;
        //                        restoreToyDetail.Reward = model.Quantity * toy.ToyPriceSale / 2;
        //                        //thêm vào Db
        //                        await _unitOfWork.GetRepository<RestoreToyDetail>().InsertAsync(restoreToyDetail);
        //                    }
        //                }
        //                //nếu ko restoreToy
        //                else
        //                {
        //                    //Tạo đơn trả đồ 
        //                    RestoreToy restoreToyNew = new RestoreToy();
        //                    restoreToyNew.ContractId = contractEntity.Id;
        //                    await _unitOfWork.GetRepository<RestoreToy>().InsertAsync(restoreToyNew);
        //                    await _unitOfWork.SaveAsync();
        //                    //Tạo đơn của từng sản phẩm
        //                    RestoreToyDetail restoreToyDetail = new RestoreToyDetail();
        //                    restoreToyDetail.RestoreToyId = restoreToyNew.Id;
        //                    restoreToyDetail.ToyQuality = model.Quantity;
        //                    restoreToyDetail.ToyName = toy.ToyName;
        //                    restoreToyDetail.ToyId = toy.Id;
        //                    restoreToyDetail.Reward = model.Quantity * toy.ToyPriceSale / 2;
        //                    //thêm vào Db

        //                    await _unitOfWork.GetRepository<RestoreToyDetail>().InsertAsync(restoreToyDetail);
        //                    await _unitOfWork.SaveAsync();
        //                }
        //            }
        //            //Cập nhật số lượng
        //            contractDetail.Quantity += model.Quantity;
        //            await _unitOfWork.GetRepository<ContractEntity>().UpdateAsync(contractEntity);

        //            //Cập nhật cái trong giỏ hàng
        //            await _unitOfWork.GetRepository<ContractDetail>().UpdateAsync(contractDetail);

        //        }
        //        //Nếu chưa có trong giỏ hàng
        //        else
        //        {
        //            ContractDetail newContractDetail = _mapper.Map<ContractDetail>(model);
        //            newContractDetail.CreatedTime = CoreHelper.SystemTimeNows;
        //            newContractDetail.CreatedBy = userId;
        //            newContractDetail.ContractId = contractEntity.Id;
        //            //Nêu true là bán lấy giá bán
        //            if (model.ContractType == true)
        //            {
        //                newContractDetail.Price = model.Quantity * toy.ToyPriceSale;
        //            }
        //            else//còn false là thuê lấy giá của thuê
        //            {
        //                newContractDetail.Price = model.Quantity * (toy.ToyPriceRent + toy.ToyPriceSale / 2);
        //                //
        //                RestoreToy restoreToy = _unitOfWork.GetRepository<RestoreToy>().Entities.FirstOrDefault(x => x.ContractId == contractEntity.Id && !x.DeletedTime.HasValue);
        //                //Nếu có restoreToy
        //                if (restoreToy != null)
        //                {
        //                    RestoreToyDetail restoreDetail = _unitOfWork.GetRepository<RestoreToyDetail>().Entities.FirstOrDefault(x => x.RestoreToyId == restoreToy.Id && x.ToyId == toy.Id);
        //                    //Nếu có restoreDetail
        //                    if (restoreDetail != null)
        //                    {
        //                        restoreDetail.ToyQuality += model.Quantity;
        //                        restoreDetail.Reward += model.Quantity * toy.ToyPriceSale / 2;
        //                        //Cập nhật lại
        //                        await _unitOfWork.GetRepository<RestoreToyDetail>().UpdateAsync(restoreDetail);
        //                    }
        //                    //Nếu ko restoreDetail
        //                    else
        //                    {
        //                        RestoreToyDetail restoreToyDetail = new RestoreToyDetail();
        //                        restoreToyDetail.RestoreToyId = restoreToy.Id;
        //                        restoreToyDetail.ToyQuality = model.Quantity;
        //                        restoreToyDetail.ToyName = toy.ToyName;
        //                        restoreToyDetail.ToyId = toy.Id;
        //                        restoreToyDetail.Reward = model.Quantity * toy.ToyPriceSale / 2;
        //                        //thêm vào Db
        //                        await _unitOfWork.GetRepository<RestoreToyDetail>().InsertAsync(restoreToyDetail);
        //                    }
        //                }
        //                //nếu ko có restoreToy
        //                else
        //                {
        //                    //Tạo đơn trả đồ 
        //                    RestoreToy restoreToyNew = new RestoreToy();
        //                    restoreToyNew.ContractId = contractEntity.Id;
        //                    //Tạo đơn của từng sản phẩm
        //                    RestoreToyDetail restoreToyDetail = new RestoreToyDetail();
        //                    restoreToyDetail.RestoreToyId = restoreToy.Id;
        //                    restoreToyDetail.ToyQuality = model.Quantity;
        //                    restoreToyDetail.ToyName = toy.ToyName;
        //                    restoreToyDetail.ToyId = toy.Id;
        //                    restoreToyDetail.Reward = model.Quantity * toy.ToyPriceSale / 2;
        //                    //thêm vào Db
        //                    await _unitOfWork.GetRepository<RestoreToy>().InsertAsync(restoreToyNew);
        //                    await _unitOfWork.GetRepository<RestoreToyDetail>().InsertAsync(restoreToyDetail);
        //                }
        //            }
        //            await _unitOfWork.GetRepository<ContractDetail>().InsertAsync(newContractDetail);

        //        }

        //        await _unitOfWork.SaveAsync();
        //    }
        //}
        public async Task CreateContractDetailAsync(CreateContractDetailModel model)
        {
            // Kiểm tra và lấy thông tin đồ chơi
            Toy toy = await _unitOfWork.GetRepository<Toy>().Entities
                .FirstOrDefaultAsync(x => x.Id == model.ToyId && !x.DeletedTime.HasValue)
                ?? throw new ErrorException((int)StatusCodeHelper.Notfound, ResponseCodeConstants.INVALID_INPUT, "Phải nhập mã đồ chơi");

            if (model.Quantity <= 0)
            {
                throw new Exception("Số lượng phải lớn hơn 0");
            }
            if (model.DateStart >= model.DateEnd)
            {
                throw new Exception("Ngày bắt đầu phải nhỏ hơn ngày kết thúc");
            }

            string userId = _httpContextAccessor.HttpContext?.Request.Cookies["UserId"];

            // Tìm hoặc tạo mới hợp đồng ở trạng thái "In Cart"
            ContractEntity contractEntity = await _unitOfWork.GetRepository<ContractEntity>().Entities
                .FirstOrDefaultAsync(x => x.UserId.ToString() == userId && x.Status == "In Cart" && !x.DeletedTime.HasValue);

            if (contractEntity == null)
            {
                contractEntity = new ContractEntity
                {
                    Status = "In Cart",
                    CreatedBy = userId,
                    CreatedTime = CoreHelper.SystemTimeNows,
                    UserId = Guid.Parse(userId),
                    TotalValue = 0
                };
                await _unitOfWork.GetRepository<ContractEntity>().InsertAsync(contractEntity);
                await _unitOfWork.SaveAsync();
            }

            // Kiểm tra loại hợp đồng (thuê/mua)
            if (model.ContractType == true) // Mua
            {
                await HandlePurchase(contractEntity, model, toy);
            }
            else // Thuê
            {
                await HandleRental(contractEntity, model, toy);
            }

            await _unitOfWork.GetRepository<ContractEntity>().UpdateAsync(contractEntity);
            await _unitOfWork.SaveAsync();
        }
        private async Task HandlePurchase(ContractEntity contractEntity, CreateContractDetailModel model, Toy toy)
        {
            ContractDetail contractDetail = await _unitOfWork.GetRepository<ContractDetail>().Entities
                .FirstOrDefaultAsync(x => x.ContractId == contractEntity.Id && !x.DeletedTime.HasValue && x.ToyId == model.ToyId && x.ContractType == model.ContractType);

            if (contractDetail != null)
            {
                // Nếu sản phẩm đã tồn tại trong giỏ hàng
                contractDetail.Quantity += model.Quantity;
                contractDetail.Price += model.Quantity * toy.ToyPriceSale;
            }
            else
            {
                // Tạo mới ContractDetail
                contractDetail = new ContractDetail
                {
                    ContractId = contractEntity.Id,
                    ToyId = model.ToyId,
                    Quantity = model.Quantity,
                    Price = model.Quantity * toy.ToyPriceSale,
                    CreatedBy = _httpContextAccessor.HttpContext?.Request.Cookies["UserId"],
                    CreatedTime = CoreHelper.SystemTimeNows,
                    ContractType = model.ContractType
                };
                await _unitOfWork.GetRepository<ContractDetail>().InsertAsync(contractDetail);
            }

            // Cập nhật giá trị tổng của hợp đồng
            contractEntity.TotalValue += model.Quantity * toy.ToyPriceSale;
            await _unitOfWork.GetRepository<ContractEntity>().UpdateAsync(contractEntity);
            await _unitOfWork.SaveAsync();
        }
        private async Task HandleRental(ContractEntity contractEntity, CreateContractDetailModel model, Toy toy)
        {
            ContractDetail contractDetail = await _unitOfWork.GetRepository<ContractDetail>().Entities
                .FirstOrDefaultAsync(x => x.ContractId == contractEntity.Id && !x.DeletedTime.HasValue && x.ToyId == model.ToyId && x.ContractType == model.ContractType);

            int rentalDays = (model.DateEnd - model.DateStart).Value.Days;
            double rentalPrice = model.Quantity * (toy.ToyPriceRent + toy.ToyPriceSale / 2) * rentalDays;

            if (contractDetail != null)
            {
                // Nếu sản phẩm đã tồn tại trong giỏ hàng
                contractDetail.Quantity += model.Quantity;
                contractDetail.Price += (int)rentalPrice;
            }
            else
            {
                // Tạo mới ContractDetail
                contractDetail = new ContractDetail
                {
                    ContractId = contractEntity.Id,
                    ToyId = model.ToyId,
                    Quantity = model.Quantity,
                    Price =(int)rentalPrice,
                    CreatedBy = _httpContextAccessor.HttpContext?.Request.Cookies["UserId"],
                    CreatedTime = CoreHelper.SystemTimeNows,
                    ContractType = model.ContractType,
                    DateStart = model.DateStart,
                    DateEnd = model.DateEnd
                };
                await _unitOfWork.GetRepository<ContractDetail>().InsertAsync(contractDetail);
                await _unitOfWork.SaveAsync();
            }

            // Tạo hoặc cập nhật RestoreToy và RestoreToyDetail
            await CreateOrUpdateRestoreToy(contractEntity.Id, model.Quantity, toy);

            // Cập nhật giá trị tổng của hợp đồng
            contractEntity.TotalValue += rentalPrice;
            await _unitOfWork.GetRepository<ContractEntity>().UpdateAsync(contractEntity);
            await _unitOfWork.SaveAsync();
        }
        private async Task CreateOrUpdateRestoreToy(string contractId, int quantity, Toy toy)
        {
            RestoreToy restoreToy = await _unitOfWork.GetRepository<RestoreToy>().Entities
                .FirstOrDefaultAsync(x => x.ContractId == contractId && !x.DeletedTime.HasValue);

            if (restoreToy == null)
            {
                // Tạo mới RestoreToy
                restoreToy = new RestoreToy
                {
                    ContractId = contractId,
                    TotalMoney = 0,
                    TotalReward = 0,
                    TotalToyQuality = 0,
                };
                await _unitOfWork.GetRepository<RestoreToy>().InsertAsync(restoreToy);
                await _unitOfWork.SaveAsync();
            }

            // Tìm hoặc tạo RestoreToyDetail
            RestoreToyDetail restoreToyDetail = await _unitOfWork.GetRepository<RestoreToyDetail>().Entities
                .FirstOrDefaultAsync(x => x.RestoreToyId == restoreToy.Id && x.ToyId == toy.Id);

            if (restoreToyDetail != null)
            {
                restoreToyDetail.ToyQuality += quantity;
                restoreToyDetail.Reward += quantity * toy.ToyPriceSale / 2;
                await _unitOfWork.GetRepository<RestoreToyDetail>().UpdateAsync(restoreToyDetail);
                await _unitOfWork.SaveAsync();
            }
            else
            {
                restoreToyDetail = new RestoreToyDetail
                {
                    RestoreToyId = restoreToy.Id,
                    ToyId = toy.Id,
                    ToyQuality = quantity,
                    ToyName = toy.ToyName,
                    Reward = quantity * toy.ToyPriceSale / 2,
                    TotalMoney = 0,
                    Compensation = 0,
                    OverdueTime = 0,
                };
                await _unitOfWork.GetRepository<RestoreToyDetail>().InsertAsync(restoreToyDetail);
                await _unitOfWork.SaveAsync();
            }
        }

        public async Task DeleteContractDetailAsync(string id)
        {
            // Lấy sản phẩm - kiểm tra sự tồn tại

            ContractDetail contractDetail = await _unitOfWork.GetRepository<ContractDetail>().Entities
                .FirstOrDefaultAsync(p => p.Id == id && !p.DeletedTime.HasValue)
                ?? throw new ErrorException((int)StatusCodeHelper.Notfound, ResponseCodeConstants.NOT_FOUND, "Contract detail not found!");
            ContractEntity contractEntity = await _unitOfWork.GetRepository<ContractEntity>().Entities.Where(p => p.Id == contractDetail.ContractId && !p.DeletedTime.HasValue).FirstOrDefaultAsync();
            // Xóa mềm
            contractDetail.DeletedTime = CoreHelper.SystemTimeNows;
            if (contractDetail.ContractType == false)
            {
                //Tìm xem có RestoreToyDetail ko
                RestoreToyDetail restoreToyDetail = await _unitOfWork.GetRepository<RestoreToyDetail>().Entities
                .Include(y => y.RestoreToy)
                .FirstOrDefaultAsync(x => x.RestoreToy.ContractId == contractDetail.ContractId && !x.DeletedTime.HasValue && x.ToyId == contractDetail.ToyId);
                //Xóa bên RestoreDetail
                if (restoreToyDetail != null)
                {
                    restoreToyDetail.DeletedTime = CoreHelper.SystemTimeNow;
                    await _unitOfWork.GetRepository<RestoreToyDetail>().UpdateAsync(restoreToyDetail);
                }
            }
            //cập nhât contract
            contractEntity.TotalValue -= contractDetail.Price;

            await _unitOfWork.GetRepository<ContractEntity>().UpdateAsync(contractEntity);
            await _unitOfWork.GetRepository<ContractDetail>().UpdateAsync(contractDetail);
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
                .Include(x => x.Toy)
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
                    ToyName = item.Toy!.ToyName!
                };
            }).ToList();

            // Trả về danh sách phân trang
            return new BasePaginatedList<ResponseContractDetailModel>(responseItems, totalCount, pageNumber, pageSize);
        }

        public async Task<ResponseContractDetailModel> GetContractDetailAsync(string id)
        {
            ContractDetail contractDetail = await _unitOfWork.GetRepository<ContractDetail>().Entities
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
            ContractEntity contractEntity = await _unitOfWork.GetRepository<ContractEntity>().Entities.FirstOrDefaultAsync(x => x.Id == contractDetail.ContractId && !x.DeletedTime.HasValue);
            //Tìm đồ chơi
            Toy toy = await _unitOfWork.GetRepository<Toy>().Entities
                .FirstOrDefaultAsync(p => p.Id == contractDetail.ToyId && !p.DeletedTime.HasValue);
            _mapper.Map(model, contractDetail);
            contractDetail.LastUpdatedTime = CoreHelper.SystemTimeNows;
            //Cập nhập lại giá
            //Nêu true là bán lấy giá bán

            int priceOld = contractDetail.Price;
            contractDetail.Price = model.Quantity * toy.ToyPriceSale;
            priceOld -= contractDetail.Price;
            contractEntity.TotalValue += priceOld;

            await _unitOfWork.GetRepository<ContractEntity>().UpdateAsync(contractEntity);
            await _unitOfWork.GetRepository<ContractDetail>().UpdateAsync(contractDetail);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateContractDetailRentAsync(string id, UpdateContractDetailRentModel model)
        {
            //Điều kiện
            if (model.Quantity <= 0)
            {
                throw new ErrorException((int)StatusCodeHelper.Notfound, ResponseCodeConstants.INVALID_INPUT, "Số lượng phải lớn hơn 0");
            }
            if (model.DateStart <= model.DateEnd)
            {
                throw new ErrorException((int)StatusCodeHelper.Notfound, ResponseCodeConstants.INVALID_INPUT, "Ngày bắt đầu phải lớn hơn ngày kết thúc");
            }
            //Tìm hợp đồng
            ContractDetail contractDetail = await _unitOfWork.GetRepository<ContractDetail>().Entities
                .FirstOrDefaultAsync(p => p.Id == id && !p.DeletedTime.HasValue)
                ?? throw new ErrorException((int)StatusCodeHelper.Notfound, ResponseCodeConstants.NOT_FOUND, "Contract detail not found!");
            ContractEntity contractEntity = await _unitOfWork.GetRepository<ContractEntity>().Entities.FirstOrDefaultAsync(x => x.Id == contractDetail.ContractId && !x.DeletedTime.HasValue);
            //Tìm đồ chơi
            Toy toy = await _unitOfWork.GetRepository<Toy>().Entities
                .FirstOrDefaultAsync(p => p.Id == contractDetail.ToyId && !p.DeletedTime.HasValue);
            _mapper.Map(model, contractDetail);
            contractDetail.LastUpdatedTime = CoreHelper.SystemTimeNows;
            //Cập nhập lại giá
            //Gán giá trị đầu
            int priceOld = contractDetail.Price;
            //giá trị = số lượng của đồ chơi * (giá thuê + giá bán/2)* giá ngày thuê
            contractDetail.Price = model.Quantity * (toy.ToyPriceRent + toy.ToyPriceSale / 2) * (model.DateStart - model.DateEnd).Value.Days;
            priceOld -= contractDetail.Price;
            contractDetail.DateStart = model.DateStart;
            contractDetail.DateEnd = model.DateEnd;
            //Cập giá giá trong contract
            contractEntity.TotalValue += priceOld;

            //Tìm xem có sẵn đơn trả chưa
            RestoreToy restoreToy = _unitOfWork.GetRepository<RestoreToy>().Entities.FirstOrDefault(x => x.ContractId == contractDetail.ContractId && !x.DeletedTime.HasValue);
            //Nếu có restoreToy
            if (restoreToy != null)
            {
                //Tìm xem đơn trả đó có Đồ chơi này chưa
                RestoreToyDetail restoreDetail = await _unitOfWork.GetRepository<RestoreToyDetail>().Entities.FirstOrDefaultAsync(x => x.RestoreToyId == restoreToy.Id && x.ToyId == toy.Id);
                //Nếu có
                if (restoreDetail != null)
                {
                    restoreDetail.ToyQuality = model.Quantity;
                    restoreDetail.Reward = model.Quantity * toy.ToyPriceSale / 2;
                    //Cập nhật lại
                    await _unitOfWork.GetRepository<RestoreToyDetail>().UpdateAsync(restoreDetail);
                }
                //Nếu ko có
                else
                {
                    RestoreToyDetail restoreToyDetail = new RestoreToyDetail();
                    restoreToyDetail.RestoreToyId = restoreToy.Id;
                    restoreToyDetail.ToyQuality = model.Quantity;
                    restoreToyDetail.ToyName = toy.ToyName;
                    restoreToyDetail.ToyId = toy.Id;
                    restoreToyDetail.Reward = model.Quantity * toy.ToyPriceSale / 2;
                    //thêm vào Db
                    await _unitOfWork.GetRepository<RestoreToyDetail>().InsertAsync(restoreToyDetail);
                }
            }
            await _unitOfWork.GetRepository<ContractEntity>().UpdateAsync(contractEntity);
            await _unitOfWork.GetRepository<ContractDetail>().UpdateAsync(contractDetail);
            await _unitOfWork.SaveAsync();
        }


    }
}
