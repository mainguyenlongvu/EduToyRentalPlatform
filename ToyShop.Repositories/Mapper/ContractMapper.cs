using AutoMapper;
using ToyShop.Contract.Repositories.Entity;
using ToyShop.ModelViews.ContractModelView;
using ToyShop.ModelViews.ToyModelViews;

namespace ToyShop.Repositories.Mapper
{
    public class ContractMapper : Profile
    {
        public ContractMapper()
        {
            CreateMap<ContractEntity, ResponseContractModel>()
                .ForMember(d => d.CustomerName, opt => opt.MapFrom(src => src.ApplicationUser.Id == src.UserId ? src.ApplicationUser.UserName : ""))
                .ForMember(d => d.StaffConfirmed, opt => opt.MapFrom(src => src.ApplicationUser.Id.ToString() == src.StaffConfirmed ? src.ApplicationUser.UserName : ""));

            CreateMap<ContractEntity, UpdateContractModel>()
            .ForMember(d => d.TotalValue, opt => opt.MapFrom(src => src.TotalValue))
            .ForMember(d => d.Status, opt => opt.MapFrom(src => src.Status));
            //.ForMember(d => d.DateStart, opt => opt.MapFrom(src => src.DateStart))
            //.ForMember(d => d.DateEnd, opt => opt.MapFrom(src => src.DateEnd));
            CreateMap<ResponseContractModel, CreateContractModel>().ReverseMap();

        }
    }
}
