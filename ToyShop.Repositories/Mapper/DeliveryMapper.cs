using AutoMapper;
using ToyShop.Contract.Repositories.Entity;
using ToyShop.ModelViews.DeliveryModelView;

namespace ToyShop.Repositories.Mapper
{
    public class DeliveryMapper : Profile
    {
        public DeliveryMapper()
        {
            CreateMap<Delivery, ResponseDeliveryModel>().ReverseMap();
            CreateMap<CreateDeliveryModel, Delivery>().ReverseMap();
        }
    }
}
