﻿using ToyShop.Contract.Services.Interface;
using ToyShop.Contract.Services;
using ToyShop.Contract.Repositories.IUOW;
using ToyShop.Repositories.UOW;
using ToyShop.Services;
using ToyShop.Services.Service;
using Microsoft.EntityFrameworkCore;
using ToyShop.Repositories.Base;
using Microsoft.AspNetCore.Identity;
using ToyShop.Repositories.Entity;
using Microsoft.AspNet.Identity;
using ToyShop.Contract.Repositories.Interface;
using ToyShop.Contract.Repositories.Entity;

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
            // Đăng ký dịch vụ IConfiguration
            services.AddSingleton<IConfiguration>(configuration);

            // Đăng ký IUserService và UserService
            services.AddScoped<IUserService, UserService>();

            // Đăng ký IUnitOfWork và UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //Đăbg ký IToySerivce và ToyService
            services.AddScoped<IToyService, ToyService>();

            // Đăng ký AutoMapper
            services.AddAutoMapper(typeof(Program));

            services.AddSession();

            // Đăng ký PasswordHasher
            services.AddScoped<IPasswordHasher<ApplicationUser>, PasswordHasher<ApplicationUser>>();
            services.AddHttpContextAccessor();
            services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<ToyShopDBContext>()
            .AddDefaultTokenProviders();
            services.IntSeedData();
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
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IContractService, ContractService>();
            services.AddScoped<IContractDetailService, ContractDetailService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IDeliveryService, DeliveryService>();
            services.AddScoped<IRestoreToyService, RestoreToyService>();
            services.AddScoped<IRestoreToyDetailService, RestoreToyDetailService>();
			services.AddScoped<IVnPayService, VnPayService>();
			services.AddScoped<GmailService>();


            // Đăng ký PasswordHasher cho ApplicationUser
            services.AddScoped<IPasswordHasher<ApplicationUser>, PasswordHasher<ApplicationUser>>();

            services.AddRazorPages();
            services.AddSignalR();

        }

        public static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
        public static void IntSeedData(this IServiceCollection services)
        {
            using var scope = services.BuildServiceProvider().CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<ToyShopDBContext>();
            var initialiser = new ApplicationDbContextInitializer(context);
            initialiser.Initialise();
        }
    }

}
