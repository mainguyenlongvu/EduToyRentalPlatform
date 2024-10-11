using AutoMapper;
using ToyShop.Contract.Repositories.Entity;
using ToyShop.ModelViews.FeedBackModelViews;

namespace ToyShop.Repositories.Mapper
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            CreateMap<FeedBack, ResponeFeedBackModel>().ReverseMap();
            CreateMap<CreateFeedBackModel, FeedBack>().ReverseMap();
            CreateMap<CreateFeedBackModel, ResponeFeedBackModel>().ReverseMap();
        }
    }
}
