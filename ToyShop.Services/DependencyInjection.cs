using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ToyShop.Contract.Repositories.Interface;
using ToyShop.Repositories.Repositories;
using Microsoft.AspNetCore.SignalR;

namespace ToyShop.Services
{
    public static class DependencyInjection
    {

        public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRepository();
            services.AddAutoMapper();
            services.AddServices(configuration);
        }
        public static void AddRepository(this IServiceCollection services)
        {
            services
               .AddScoped<IUnitOfWork, UnitOfWork>();
        }
        private static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
			
        }
    }
}
