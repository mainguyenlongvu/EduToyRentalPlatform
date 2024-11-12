using AutoMapper;
using ToyShop.Contract.Repositories.Entity;
using ToyShop.Contract.Services.Interface;
using ToyShop.Core.Base;
using ToyShop.ModelViews.TransactionModelView;
using Microsoft.EntityFrameworkCore;
using ToyShop.Contract.Repositories.Interface;

namespace ToyShop.Services.Service
{
    public class TransactionService : ITransactionService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public TransactionService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
            _mapper = mapper;
        }

		public async Task<bool> Delete(string id)
		{
			try
			{
				var transaction = _unitOfWork.GetRepository<Transaction>().Entities.FirstOrDefault(x=>!x.DeletedTime.HasValue && x.Id == id) ?? throw new KeyNotFoundException($"Transaction with id {id} not found");
				
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
				var transactions = await _unitOfWork.GetRepository<Transaction>().Entities.Where(x=>!x.DeletedTime.HasValue).ToListAsync();
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
                return  _mapper.Map<ResponseTransactionModel>(transaction);
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
                var query = _unitOfWork.GetRepository<Transaction>().Entities.Where(x=>!x.DeletedTime.HasValue);

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
				var contract = await _unitOfWork.GetRepository<ContractEntity>().Entities.FirstOrDefaultAsync(x=>x.Id == transactionCreate.ContractId && !x.DeletedTime.HasValue) ?? throw new KeyNotFoundException("Contract not found.");
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
                // Retrieve the existing transaction
                var existingTransaction = await _unitOfWork.GetRepository<Transaction>()
                    .Entities
                    .FirstOrDefaultAsync(d => d.TranCode == tranCode && !d.DeletedTime.HasValue)
                    ?? throw new KeyNotFoundException("Transaction not found.");

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

    }
}
