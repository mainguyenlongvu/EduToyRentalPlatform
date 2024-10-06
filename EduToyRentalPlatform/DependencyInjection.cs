using ToyShop.Contract.Services.Interface;
using ToyShop.Contract.Services;
using ToyShop.Contract.Repositories.IUOW;
using ToyShop.Repositories.UOW;
using ToyShop.Services;
using ToyShop.Services.Service;
using Microsoft.EntityFrameworkCore;
using ToyShop.Repositories.Base;

namespace ToyShop
{
    public static class DependencyInjection
    {
        public static void AddConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigRoute();
            services.AddDatabase(configuration);
            services.AddIdentity();
            services.AddInfrastructure(configuration);
            services.AddServices(configuration);
            services.AddDistributedMemoryCache();
            services.AddAutoMapper();
            services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });
        }
        public static void ConfigRoute(this IServiceCollection services)
        {
            services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
            });
        }
        public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ToyShopDBContext>(options =>
            {
                options.UseLazyLoadingProxies()
                       .UseSqlServer(configuration.GetConnectionString("DBConnection"));
            });
        }
        public static void AddIdentity(this IServiceCollection services)
        {


        }
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IToyService, ToyService>();
            services.AddScoped<IFeedBackService, FeedBackService>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IMessageService, MessageService>();

            services.AddRazorPages();

        }

        public static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

    }

}
