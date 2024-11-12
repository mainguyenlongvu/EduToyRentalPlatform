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
using ToyShop.ModelViews.TopUpModel;
using Microsoft.AspNetCore.Http;
using ToyShop.Repositories.Entity;

namespace ToyShop.Contract.Services.Interface
{
    public class ContractService : IContractService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public ContractService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
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

        public async Task<BasePaginatedList<ContractEntity>> GetContractsAsync(int pageNumber, int pageSize, string searchTerm = null)
        {
            // Validate page number and page size
            pageNumber = Math.Max(pageNumber, 1);
            pageSize = Math.Max(pageSize, 5);

            // Start building the query for non-deleted contracts
            IQueryable<ContractEntity> contractsQuery = _unitOfWork.GetRepository<ContractEntity>().Entities
                .Where(p => !p.DeletedTime.HasValue) // Filter out deleted contracts
                .Include(p => p.ApplicationUser); // Include ApplicationUser navigation property

            // If a search term is provided, filter the results
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                contractsQuery = contractsQuery.Where(p =>
                    p.Status.Contains(searchTerm) || // Filter by status
                    p.ApplicationUser.FullName.Contains(searchTerm) // Filter based on User's Full Name
                );
            }

            // Order by CreatedTime
            contractsQuery = contractsQuery.OrderByDescending(p => p.DateCreated); // Assuming DateCreated is the field to order by

            // Get total count for pagination
            int totalCount = await contractsQuery.CountAsync();

            // Apply pagination
            List<ContractEntity> paginatedContracts = await contractsQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Return the paginated list
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
        public async Task DirectPaymentAsync(string id, DirectPaymentModel model)
        {
            //Lấy id người dùng
            string userId = _httpContextAccessor.HttpContext?.Request.Cookies["UserId"]!;
            //Lấy hợp đồng
            ContractEntity contract = await _unitOfWork.GetRepository<ContractEntity>().Entities
                .FirstOrDefaultAsync(p => p.Id == id && !p.DeletedTime.HasValue)
                ?? throw new ErrorException((int)StatusCodeHelper.Notfound, ResponseCodeConstants.NOT_FOUND, "Contract not found!");

            if (model == null)
                throw new ArgumentNullException(nameof(model), "Update model cannot be null.");

            _mapper.Map(model, contract);
            contract.LastUpdatedTime = CoreHelper.SystemTimeNows;
            contract.Status = "Done";
            //Tạo phương thức thanh toán
            Transaction transaction = new Transaction
            {
                Status = "Processing",
                CreatedBy = userId,
                ContractId = contract.Id,
                DateCreated = CoreHelper.SystemTimeNows,
                Method = false,
                TranCode = GenerateBillCode(),
                LastUpdatedTime = CoreHelper.SystemTimeNows,
            };
            await _unitOfWork.GetRepository<ContractEntity>().UpdateAsync(contract);
            await _unitOfWork.GetRepository<Transaction>().InsertAsync(transaction);
            await _unitOfWork.SaveAsync();
        }
        private int GenerateBillCode()
        {
            Random random = new Random();
            return random.Next(100000, 1000000); // Generates a 6-digit number between 100000 and 999999
        }

        public async Task PayByWalletAsync(string id, PayByWalletModel model)
        {
            //Lấy id người dùng
            string userId = _httpContextAccessor.HttpContext?.Request.Cookies["UserId"]!;
            ContractEntity contract = await _unitOfWork.GetRepository<ContractEntity>().Entities
                .FirstOrDefaultAsync(p => p.Id == id && !p.DeletedTime.HasValue)
                ?? throw new ErrorException((int)StatusCodeHelper.Notfound, ResponseCodeConstants.NOT_FOUND, "Contract not found!");

            if (model == null)
                throw new ArgumentNullException(nameof(model), "Update model cannot be null.");

            _mapper.Map(model, contract);
            contract.LastUpdatedTime = CoreHelper.SystemTimeNows;

            //tìm người dùng
            ApplicationUser user = await _unitOfWork.GetRepository<ApplicationUser>().Entities.FirstOrDefaultAsync(x => x.Id.ToString() == userId);
            if (user.Money < model.TotalValue)
            {
                throw new ErrorException((int)StatusCodeHelper.BadRequest, ResponseCodeConstants.BADREQUEST, "Tài khoản của bạn không đủ!");
            }
            //trừ tiền trong tài khoản
            user.Money -= (int)model.TotalValue;

            //Tạo phương thức thanh toán
            Transaction transaction = new Transaction
            {
                Status = "Done",
                CreatedBy = userId,
                ContractId = contract.Id,
                DateCreated = CoreHelper.SystemTimeNows,
                Method = false,
                TranCode = GenerateBillCode(),
                LastUpdatedTime = CoreHelper.SystemTimeNows,
            };
            await _unitOfWork.GetRepository<ApplicationUser>().UpdateAsync(user);
            await _unitOfWork.GetRepository<ContractEntity>().UpdateAsync(contract);
            await _unitOfWork.GetRepository<Transaction>().InsertAsync(transaction);
            await _unitOfWork.SaveAsync();
        }

        //Chưa code 
        public async Task CancelContractAsync(string id, UpdateContractModel model)
        {
            //được hủy đơn để hoàn tiền trong vòng 1 ngày: điều kiện phải trước thời gian thuê
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
            return await _unitOfWork.GetRepository<ContractEntity>().Entities.Where(X => !X.DeletedTime.HasValue).ToListAsync();
        }

        public Task<bool> CreateTopUpAsync(CreateTopUpModel model)
        {
            throw new NotImplementedException();
        }
    }
}