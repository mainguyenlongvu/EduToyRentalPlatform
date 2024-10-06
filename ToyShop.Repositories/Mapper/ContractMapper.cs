using AutoMapper;
using ToyShop.ModelViews.ContractModelView;
using ToyShop.ModelViews.ToyModelViews;

namespace ToyShop.Repositories.Mapper
{
    public class ContractMapper:Profile
    {
        public ContractMapper()
        {
            CreateMap<ToyShop.Contract.Repositories.Entity.ContractEntity, ResponseContractModel>()
                .ForMember(d => d.CustomerName, opt => opt.MapFrom(src => src.ApplicationUser.Id == src.UserId ? src.ApplicationUser.UserName:""))
                .ForMember(d => d.StaffConfirmed, opt => opt.MapFrom(src => src.ApplicationUser.Id.ToString() == src.StaffConfirmed? src.ApplicationUser.UserName:""))
                .ForMember(d => d.ToyName, opt => opt.MapFrom(src => src.Toy.Id == src.ToyId? src.Toy.ToyName:""));
        }
    }
}
