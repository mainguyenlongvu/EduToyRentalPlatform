using System;

namespace ToyShop.ModelViews.TransactionModelView
{
    public class UpdateTransactionModel
    {
        public int TranCode { get; set; } 
        public int? ContractId { get; set; } 
        public string Status { get; set; }

    }
}
