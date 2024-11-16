using AutoMapper;
using ToyShop.Contract.Repositories.Entity;
using ToyShop.Contract.Services.Interface;
using ToyShop.Core.Base;
using ToyShop.ModelViews.TransactionModelView;
using Microsoft.EntityFrameworkCore;
using ToyShop.Contract.Repositories.Interface;
using ToyShop.Repositories.Entity;
using Microsoft.AspNetCore.Http;
using ToyShop.ModelViews.GmailModel;
using ToyShop.ModelViews.ContractModelView;
using System.Diagnostics.Contracts;

namespace ToyShop.Services.Service
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly GmailService _gmailService;

		public TransactionService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, GmailService gmailService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_httpContextAccessor = httpContextAccessor;
			_gmailService = gmailService;
		}

		public async Task<bool> Delete(string id)
        {
            try
            {
                var transaction = _unitOfWork.GetRepository<Transaction>().Entities.FirstOrDefault(x => !x.DeletedTime.HasValue && x.Id == id) ?? throw new KeyNotFoundException($"Transaction with id {id} not found");

                _unitOfWork.GetRepository<Transaction>().Delete(transaction.Id);
                await _unitOfWork.SaveAsync();
                return false;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to delete transaction. Please try again later.", ex);
            }
        }

        public async Task<IEnumerable<ResponseTransactionModel>> GetAll()
        {
            try
            {
                var transactions = await _unitOfWork.GetRepository<Transaction>().Entities.Where(x => !x.DeletedTime.HasValue).ToListAsync();
                return transactions.Select(d => _mapper.Map<ResponseTransactionModel>(d));
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to retrieve transactions. Please try again later.", ex);
            }
        }

        public async Task<ResponseTransactionModel> GetById(string id)
        {
            try
            {
                var transaction = _unitOfWork.GetRepository<Transaction>().Entities.FirstOrDefault(x => !x.DeletedTime.HasValue && x.Id == id) ?? throw new KeyNotFoundException($"Transaction with id {id} not found");
                return _mapper.Map<ResponseTransactionModel>(transaction);
            }
            catch (Exception ex) when (ex is KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to retrieve transaction. Please try again later.", ex);
            }
        }

        public async Task<BasePaginatedList<ResponseTransactionModel>> GetPaging(int page, int pageSize)
        {
            try
            {
                // Validate input parameters
                if (page < 1 || pageSize < 1)
                {
                    throw new ArgumentException("Invalid page or pageSize value.");
                }

                // Get the repository and prepare the query
                var query = _unitOfWork.GetRepository<Transaction>().Entities.Where(x => !x.DeletedTime.HasValue);

                // Get total count asynchronously
                var totalItems = await query.CountAsync();

                // Get the paginated items
                var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

                // Map items to response model
                var transactionResponses = items.Select(_mapper.Map<ResponseTransactionModel>).ToList();

                // Return the paginated list
                return new BasePaginatedList<ResponseTransactionModel>(transactionResponses, totalItems, page, pageSize);
            }
            catch (ArgumentException)
            {
                // Rethrow ArgumentExceptions to preserve the stack trace
                throw;
            }
            catch (Exception ex)
            {
                // Log exception details if necessary (optional)
                // Log.Error("An error occurred while fetching paginated deliveries.", ex); // Example logging

                throw new InvalidOperationException("An error occurred while fetching paginated deliveries.", ex);
            }
        }
        public async Task<CreateTransactionModel> Insert(CreateTransactionModel transactionCreate)
        {
            ArgumentNullException.ThrowIfNull(transactionCreate);
            if (transactionCreate.ContractId == null)
            {
                throw new ArgumentNullException("ContractId is required.");
            }
            try
            {
                var contract = await _unitOfWork.GetRepository<ContractEntity>().Entities.FirstOrDefaultAsync(x => x.Id == transactionCreate.ContractId && !x.DeletedTime.HasValue) ?? throw new KeyNotFoundException("Contract not found.");
                Transaction transaction = _mapper.Map<Transaction>(transactionCreate);

                await _unitOfWork.GetRepository<Transaction>().InsertAsync(transaction);
                await _unitOfWork.SaveAsync();

                return _mapper.Map<CreateTransactionModel>(transaction);
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to insert delivery. Please try again later.", ex);
            }
        }
        public async Task<UpdateTransactionModel> Update(int tranCode, CreateTransactionModel transactionDTO)
        {
            ArgumentNullException.ThrowIfNull(transactionDTO);

            if (transactionDTO.ContractId == null)
            {
                throw new ArgumentNullException(nameof(transactionDTO.ContractId), "ContractId is required.");
            }

            try
            {
                //Lấy id người dùng
                string userId = _httpContextAccessor.HttpContext?.Request.Cookies["UserId"]!;
                // Retrieve the existing transaction
                var existingTransaction = await _unitOfWork.GetRepository<Transaction>()
                    .Entities
                    .FirstOrDefaultAsync(d => d.TranCode == tranCode && !d.DeletedTime.HasValue)
                    ?? throw new KeyNotFoundException("Transaction not found.");
                //tìm người dùng
                if (transactionDTO.Status == "Completed")
                {
                    ApplicationUser user = await _unitOfWork.GetRepository<ApplicationUser>().Entities.FirstOrDefaultAsync(x => x.Id.ToString().Equals( userId))!;
					if (user != null)
					{
						string body = $"<div style=\"font-family: Helvetica,Arial,sans-serif;min-width:1000px;overflow:auto;line-height:2\">\r\n  <div style=\"margin:50px auto;width:70%;padding:20px 0\">\r\n    <div style=\"border-bottom:1px solid #eee\">\r\n      <a href=\"\" style=\"font-size:1.4em;color: #ee0000;text-decoration:none;font-weight:600\">EduToyRent Platform</a>\r\n    </div>\r\n    <p style=\"font-size:1.1em\">Chào bạn,</p>\r\n    <p>Hóa đơn của bạn thanh toán thành công. Cảm ơn bạn đã lựa chọn dịch vụ của chúng tôi</p>\r\n    <p style=\"font-size:0.9em;\">Thân,<br />EduToyRent Staff</p>\r\n    <hr style=\"border:none;border-top:1px solid #eee\" />\r\n    <div style=\"float:right;padding:8px 0;color:#aaa;font-size:0.8em;line-height:1;font-weight:300\">\r\n      <p>EduToyRent Platform</p>\r\n      <p>Ho Chi Minh City</p>\r\n      <p>Vietnam</p>\r\n    </div>\r\n  </div>\r\n</div>";
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
					}
                }

                _mapper.Map(transactionDTO, existingTransaction);

                _unitOfWork.GetRepository<Transaction>().Update(existingTransaction);
                await _unitOfWork.SaveAsync();

                return _mapper.Map<UpdateTransactionModel>(existingTransaction);
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while updating the transaction.", ex);
            }
        }


		public async Task<bool> ProcessPurchaseDirect(CreateTransactionModel tranModel)
		{
			var existingContract = await _unitOfWork.GetRepository<ContractEntity>().Entities.FirstOrDefaultAsync(x => x.Id == tranModel.ContractId && !x.DeletedTime.HasValue)
				?? throw new KeyNotFoundException("Contract not found.");

			existingContract.Status = "Processing";
			await _unitOfWork.GetRepository<ContractEntity>().UpdateAsync(existingContract);
			////
			tranModel.Status = "Processing";
			await Insert(tranModel);
			await _unitOfWork.SaveAsync();

			return true;
		}

		public async Task<bool> ProcessPurchaseVnPay(CreateTransactionModel tranModel, string userId)
		{
			var existingContract = await _unitOfWork.GetRepository<ContractEntity>().Entities.FirstOrDefaultAsync(x => x.Id == tranModel.ContractId && !x.DeletedTime.HasValue) 
				?? throw new KeyNotFoundException("Contract not found.");

			existingContract.Status = "Done";
			await _unitOfWork.GetRepository<ContractEntity>().UpdateAsync(existingContract);
			////
			await Insert(tranModel);
			await _unitOfWork.SaveAsync();

			return true;
		}

		public async Task<bool> ProcessPurchaseWallet(CreateTransactionModel tranModel, string userId)
		{
			var existingContract = await _unitOfWork.GetRepository<ContractEntity>().Entities.FirstOrDefaultAsync(x => x.Id == tranModel.ContractId && !x.DeletedTime.HasValue)
				?? throw new KeyNotFoundException("Contract not found.");

			existingContract.Status = "Done";
			await _unitOfWork.GetRepository<ContractEntity>().UpdateAsync(existingContract);
			////
			var existingUser = await _unitOfWork.GetRepository<ApplicationUser>().Entities.FirstOrDefaultAsync(x => x.Id.Equals(userId))
				?? throw new KeyNotFoundException("User not found.");

			if (existingUser.Money < Convert.ToInt32(existingContract.TotalValue)) // hết xiền
				return false;
			existingUser.Money -= Convert.ToInt32(existingContract.TotalValue);
			
			await _unitOfWork.GetRepository<ApplicationUser>().UpdateAsync(existingUser);
			////
			await Insert(tranModel);
			await _unitOfWork.SaveAsync();

			return true;
		}

		public async Task<bool> ProcessTopUpVnPay(CreateTransactionModel tranModel, string userId)
		{
			var existingContract = await _unitOfWork.GetRepository<ContractEntity>().Entities.FirstOrDefaultAsync(x => x.Id == tranModel.ContractId && !x.DeletedTime.HasValue)
				?? throw new KeyNotFoundException("Contract not found.");

			existingContract.Status = "Done";
			await _unitOfWork.GetRepository<ContractEntity>().UpdateAsync(existingContract);
			////
			var existingUser = await _unitOfWork.GetRepository<ApplicationUser>().Entities.FirstOrDefaultAsync(x => x.Id.Equals(userId))
				?? throw new KeyNotFoundException("User not found.");

			existingUser.Money += Convert.ToInt32(existingContract.TotalValue);

			await _unitOfWork.GetRepository<ApplicationUser>().UpdateAsync(existingUser);
			////
			await Insert(tranModel);
			await _unitOfWork.SaveAsync();

			return true;
		}
	}
}
