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
				if (_unitOfWork.GetRepository<Transaction>().GetById(id) is null)
				{
					throw new KeyNotFoundException($"Transaction with id {id} not found");
				}
                _unitOfWork.GetRepository<Transaction>().Delete(id);
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
				var transactions = await _unitOfWork.GetRepository<Transaction>().GetAllAsync();
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
				var transaction = await _unitOfWork.GetRepository<Transaction>().GetByIdAsync(id);
				return transaction == null ? throw new KeyNotFoundException($"Transaction with id {id} not found.")
					: _mapper.Map<ResponseTransactionModel>(transaction);
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
                var query = _unitOfWork.GetRepository<Transaction>().Entities;

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
				var contract = await _unitOfWork.GetRepository<ToyShop.Contract.Repositories.Entity.ContractEntity>()
					.GetByIdAsync(transactionCreate.ContractId) ?? throw new KeyNotFoundException("Contract not found.");
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

		public async Task<CreateTransactionModel> Update(int tranCode, CreateTransactionModel transactionDTO)
		{
			ArgumentNullException.ThrowIfNull(transactionDTO);
			if (transactionDTO.ContractId == null)
			{
				throw new ArgumentNullException(nameof(transactionDTO.ContractId), "ContractId is required.");
            }
            try
            {
				Transaction existingTransaction = _unitOfWork.GetRepository<Transaction>().Entities.AsNoTracking().FirstOrDefault(d => d.TranCode == tranCode)
					?? throw new KeyNotFoundException("Transaction not found.");

				Contract.Repositories.Entity.ContractEntity contract = await _unitOfWork.GetRepository<Contract.Repositories.Entity.ContractEntity>()
					.GetByIdAsync(transactionDTO.ContractId)
					?? throw new KeyNotFoundException($"Contract with id {transactionDTO.ContractId} not found.");

                CreateTransactionModel createModel = _mapper.Map<CreateTransactionModel>(transactionDTO);
                createModel.TranCode = tranCode;

                _unitOfWork.GetRepository<Transaction>().Update(existingTransaction);

				await _unitOfWork.SaveAsync();

				return _mapper.Map<CreateTransactionModel>(existingTransaction);
			}
			catch (KeyNotFoundException)
			{
				throw;
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException(ex.Message, ex);
			}
		}
	}
}
