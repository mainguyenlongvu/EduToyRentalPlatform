using AutoMapper;
using ToyShop.Contract.Repositories.Entity;
using ToyShop.ModelViews.DeliveryModelView;
using ToyShop.ModelViews.TransactionModelView;

namespace ToyShop.Repositories.Mapper
{
    public class TransactionMapper : Profile
    {
        public TransactionMapper()
        {
            CreateMap<Transaction, ResponseTransactionModel>().ReverseMap();
            CreateMap<CreateTransactionModel, Delivery>().ReverseMap();
        }
    }
}
