﻿using ToyShop.Core.Base;
using ToyShop.ModelViews.TransactionModelView;

namespace ToyShop.Contract.Services.Interface
{
    public interface ITransactionService
    {
        Task<CreateTransactionModel> Insert(CreateTransactionModel transactionDTO);

        Task<UpdateTransactionModel> Update(int tranCode, CreateTransactionModel transactionDTO);

        Task<bool> Delete(string tranCode);

        Task<ResponseTransactionModel> GetById(string tranCode);

        Task<IEnumerable<ResponseTransactionModel>> GetAll();

        Task<BasePaginatedList<ResponseTransactionModel>> GetPaging(int page, int pageSize);
    }
}
