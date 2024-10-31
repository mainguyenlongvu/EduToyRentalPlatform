using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToyShop.Contract.Repositories.Entity;
using ToyShop.ModelViews.FeedBackModelViews;
using ToyShop.ModelViews.RestoreToyModelViews;

namespace ToyShop.Repositories.Mapper
{
    internal class RestoreToyMapper : Profile
    {
        public RestoreToyMapper()
        {
            CreateMap<RestoreToy, UpdateRestoreModel>().ReverseMap();
        }
    }
}
