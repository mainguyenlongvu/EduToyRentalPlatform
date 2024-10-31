using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToyShop.Contract.Repositories.Entity;
using ToyShop.ModelViews.RestoreToyDetailModelViews;
using ToyShop.ModelViews.RestoreToyModelViews;

namespace ToyShop.Repositories.Mapper
{
    public class RestoreToyDetailMapper : Profile
    {
        public RestoreToyDetailMapper()
        {
            CreateMap<RestoreToyDetail, UpdateRestoreDetailModel>().ReverseMap();

        }
    }
}
