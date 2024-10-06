
using AutoMapper;
using ToyShop.Contract.Repositories.Entity;
using ToyShop.ModelViews.ToyModelViews;

namespace ToyShop.Repositories.Mapper
{
   public class ToyMapping : Profile
    {
        public ToyMapping() {
            CreateMap<Toy, ResponeToyModel>
                ();
            CreateMap<Toy, CreateToyModel>
               ();
            CreateMap<CreateToyModel, Toy>();
        }
    }
}
