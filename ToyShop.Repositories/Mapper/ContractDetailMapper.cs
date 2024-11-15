using AutoMapper;
using ToyShop.Contract.Repositories.Entity;
using ToyShop.ModelViews.ContractDetailModelView;
using ToyShop.ModelViews.ContractModelView;
using ToyShop.ModelViews.ToyModelViews;

namespace ToyShop.Repositories.Mapper
{
    public class ContractDetailMapper : Profile
    {
        public ContractDetailMapper()
        {
            CreateMap<ContractDetail, ResponseContractDetailModel>()
                .ForMember(d => d.ToyName, opt => opt.MapFrom(src => src.Toy.Id == src.ToyId ? src.Toy.ToyName : ""));

            CreateMap<ContractDetail, UpdateContractDetailModel>().ReverseMap();
            CreateMap<ContractDetail, UpdateContractDetailRentModel>().ReverseMap();
            CreateMap<ContractDetail, CreateContractDetailModel>().ReverseMap();
            CreateMap<ResponseContractDetailModel, CreateContractDetailModel>().ReverseMap();

        }
    }
}
