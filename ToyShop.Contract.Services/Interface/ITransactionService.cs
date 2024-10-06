using ToyShop.Core.Base;
using ToyShop.Core.Utils;
using ToyShop.ModelViews.TransactionModelView;

namespace ToyShop.Contract.Services.Interface
{
	public interface ITransactionService
	{
		Task<CreateTransactionModel> Insert(CreateTransactionModel transactionDTO);

		Task<CreateTransactionModel> Update(int tranCode, CreateTransactionModel transactionDTO);

		Task<bool> Delete(string id);

		Task<ResponseTransactionModel> GetById(string id);

		Task<IEnumerable<ResponseTransactionModel>> GetAll();

		Task<BasePaginatedList<ResponseTransactionModel>> GetPaging(int page, int pageSize);
	}
}
