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
using ToyShop.Services.Service;
using ToyShop.ModelViews.GmailModel;
using ToyShop.Contract.Repositories.PaggingItems;

namespace ToyShop.Contract.Services.Interface
{
    public class ContractService : IContractService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly GmailService _gmailService;

        public ContractService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, GmailService gmailService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _gmailService = gmailService;
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
                .Where(p => !p.DeletedTime.HasValue && p.Status != "In Cart") // Filter out deleted contracts
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
        public async Task DirectPaymentAsync(string id)
        {
            //Lấy id người dùng
            string userId = _httpContextAccessor.HttpContext?.Request.Cookies["UserId"]!;
            //Lấy hợp đồng
            ContractEntity contract = await _unitOfWork.GetRepository<ContractEntity>().Entities
                .FirstOrDefaultAsync(p => p.Id == id && !p.DeletedTime.HasValue)
                ?? throw new ErrorException((int)StatusCodeHelper.Notfound, ResponseCodeConstants.NOT_FOUND, "Contract not found!");

            ApplicationUser user = await _unitOfWork.GetRepository<ApplicationUser>().Entities.FirstOrDefaultAsync(x=>x.Id.ToString() == userId)!;
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
            string body = $"<div style=\"font-family: Helvetica,Arial,sans-serif;min-width:1000px;overflow:auto;line-height:2\">\r\n  <div style=\"margin:50px auto;width:70%;padding:20px 0\">\r\n    <div style=\"border-bottom:1px solid #eee\">\r\n      <a href=\"\" style=\"font-size:1.4em;color: #ee0000;text-decoration:none;font-weight:600\">EduToyRent Platform</a>\r\n    </div>\r\n    <p style=\"font-size:1.1em\">Chào bạn,</p>\r\n    <p>Đơn hàng của bạn đã đặt thành công, vui lòng đến cửa hàng để thanh toán</p>\r\n    <p style=\"font-size:0.9em;\">Xin cảm ơn,<br />EduToyRent Staff</p>\r\n    <hr style=\"border:none;border-top:1px solid #eee\" />\r\n    <div style=\"float:right;padding:8px 0;color:#aaa;font-size:0.8em;line-height:1;font-weight:300\">\r\n      <p>EduToyRent Platform</p>\r\n      <p>Ho Chi Minh City</p>\r\n      <p>Vietnam</p>\r\n    </div>\r\n  </div>\r\n</div>";
            EmailRequestModel emailRequestModel = new EmailRequestModel
            {
                EmailBody = body,
                IsHtml = true,
                EmailSubject = "Thông báo hóa đơn",
                ReceiverEmail = user.Email,
            };
            //Kiểm tra gửi email có thành công không
            if (!_gmailService.SendEmailSingle(emailRequestModel))
            {
                throw new Exception("Gửi email thất bại");
            }
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
                Status = "Not Received",
                CreatedBy = userId,
                ContractId = contract.Id,
                DateCreated = CoreHelper.SystemTimeNows,
                Method = false,
                TranCode = GenerateBillCode(),
                LastUpdatedTime = CoreHelper.SystemTimeNows,
            };
            string body = $"<div style=\"font-family: Helvetica,Arial,sans-serif;min-width:1000px;overflow:auto;line-height:2\">\r\n  <div style=\"margin:50px auto;width:70%;padding:20px 0\">\r\n    <div style=\"border-bottom:1px solid #eee\">\r\n      <a href=\"\" style=\"font-size:1.4em;color: #ee0000;text-decoration:none;font-weight:600\">EduToyRent Platform</a>\r\n    </div>\r\n    <p style=\"font-size:1.1em\">Chào bạn,</p>\r\n    <p>Hóa đơn của bạn thanh toán thành công. Bạn vui lòng đến cửa hàng để nhận.</p>\r\n    <p style=\"font-size:0.9em;\">Thân,<br />EduToyRent Staff</p>\r\n    <hr style=\"border:none;border-top:1px solid #eee\" />\r\n    <div style=\"float:right;padding:8px 0;color:#aaa;font-size:0.8em;line-height:1;font-weight:300\">\r\n      <p>EduToyRent Platform</p>\r\n      <p>Ho Chi Minh City</p>\r\n      <p>Vietnam</p>\r\n    </div>\r\n  </div>\r\n</div>";
            EmailRequestModel emailRequestModel = new EmailRequestModel
            {
                EmailBody = body,
                IsHtml = true,
                EmailSubject = "Thông báo hóa đơn",
                ReceiverEmail = user.Email,
            };
            //Kiểm tra gửi email có thành công không
            if (!_gmailService.SendEmailSingle(emailRequestModel))
            {
                throw new Exception("Gửi email thất bại");
            }
            await _unitOfWork.GetRepository<ApplicationUser>().UpdateAsync(user);
            await _unitOfWork.GetRepository<ContractEntity>().UpdateAsync(contract);
            await _unitOfWork.GetRepository<Transaction>().InsertAsync(transaction);
            await _unitOfWork.SaveAsync();
        }
        public async Task CancelContractAsync(string id)
        {
            //được hủy đơn để hoàn tiền trong vòng 1 ngày: điều kiện phải trước thời gian thuê
            ContractEntity contract = await _unitOfWork.GetRepository<ContractEntity>().Entities
                .FirstOrDefaultAsync(p => p.Id == id && !p.DeletedTime.HasValue && p.Status == "Not Received")
                ?? throw new ErrorException((int)StatusCodeHelper.Notfound, ResponseCodeConstants.NOT_FOUND, "Contract not found!");

            contract.Status = "Canceled";
            contract.LastUpdatedTime = CoreHelper.SystemTimeNows;
            //Lấy Id người dùng
            string userId = _httpContextAccessor.HttpContext?.Request.Cookies["UserId"];
            ApplicationUser user = await _unitOfWork.GetRepository<ApplicationUser>().Entities.FirstOrDefaultAsync(x=>x.Id.ToString() == userId);
            //Hoàn tiền cho người dùng
            user.Money += (int)contract.TotalValue;

            await _unitOfWork.GetRepository<ApplicationUser>().UpdateAsync(user);
            await _unitOfWork.GetRepository<ContractEntity>().UpdateAsync(contract);
            await _unitOfWork.SaveAsync();
        }

        // Fetching all contracts without soft delete filtering (consider if you need this)
        public async Task<List<ContractEntity>> GetAllContractsAsync()
        {
            return await _unitOfWork.GetRepository<ContractEntity>().Entities.Where(X => !X.DeletedTime.HasValue).ToListAsync();
        }

        public async Task<PaginatedList<ContractEntity>> GetAllContractsByUserIdAsync(string userId, string searchTerm)
        {
            // Validate page number and page size
            int pageNumber = 1;
            int pageSize = 10;
  

            // Start building the query for non-deleted contracts
            IQueryable<ContractEntity> contractsQuery = _unitOfWork.GetRepository<ContractEntity>().Entities
                .Where(p => !p.DeletedTime.HasValue && p.Status != "In Cart" && p.UserId.ToString() == userId) // Filter out deleted contracts
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
            return new PaginatedList<ContractEntity>(paginatedContracts, totalCount, pageNumber, pageSize);
        }

        public Task<bool> CreateTopUpAsync(CreateTopUpModel model)
        {
            throw new NotImplementedException();
        }
        public async Task<ContractEntity> GetContractDetailInCart()
        {
            string userId = _httpContextAccessor.HttpContext?.Request.Cookies["UserId"];

            var contractEntity = await _unitOfWork.GetRepository<ContractEntity>()
                    .Entities
                    .Include(c => c.ContractDetails)
                        .ThenInclude(d => d.Toy)
                    .FirstOrDefaultAsync(x => x.UserId.ToString() == userId && x.Status == "In Cart" && !x.DeletedTime.HasValue);
            return contractEntity;
        }
    }
}