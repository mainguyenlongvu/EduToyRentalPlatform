using AutoMapper;
using ToyShop.Contract.Repositories.Entity;
using ToyShop.ModelViews.ContractModelView;
using ToyShop.ModelViews.ToyModelViews;

namespace ToyShop.Repositories.Mapper
{
    public class ContractDetailMapper : Profile
    {
        public ContractDetailMapper()
        {
            CreateMap<ContractDetail, ResponseContractModel>()
                .ForMember(d => d.ToyName, opt => opt.MapFrom(src => src.Toy.Id == src.ToyId ? src.Toy.ToyName : ""));

            CreateMap<ContractDetail, UpdateContractModel>().ReverseMap();
            CreateMap<ResponseContractModel, CreateContractModel>().ReverseMap();

        }
    }
}
