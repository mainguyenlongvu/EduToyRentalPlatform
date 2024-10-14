using AutoMapper;
using Microsoft.Extensions.Configuration;
using ToyShop.Contract.Repositories.Entity;
using ToyShop.Contract.Services.Interface;
using ToyShop.Core.Utils;
using ToyShop.ModelViews.UserModelViews;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ToyShop.Contract.Repositories.Interface;
using ToyShop.Repositories.Entity;
using Microsoft.AspNetCore.Http;

namespace ToyShop.Services.Service
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly IConfiguration _configuration;

        public UserService(IHttpContextAccessor _httpContextAccessor1, IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration, IPasswordHasher<ApplicationUser> passwordHasher)
        {
            _httpContextAccessor = _httpContextAccessor1;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
        }
        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            var userRepository = _unitOfWork.GetRepository<ApplicationUser>();
            if (userRepository == null)
            {
                throw new Exception("User repository is null.");
            }

            return await userRepository.Entities.FirstOrDefaultAsync(u => u.Id.ToString() == userId && !u.DeletedTime.HasValue);
        }

        public async Task<string> LoginAsync(LoginModel model)
        {
            var userRepository = _unitOfWork.GetRepository<ApplicationUser>();
            if (userRepository == null)
            {
                throw new Exception("User repository is null.");
            }

            // Tìm người dùng trong cơ sở dữ liệu
            var user = await userRepository.Entities
                .FirstOrDefaultAsync(u => u.Email.ToLower() == model.Email.ToLower() && !u.DeletedTime.HasValue);

            // Kiểm tra nếu người dùng không tồn tại
            if (user == null)
            {
                throw new Exception("Người dùng không tồn tại.");
            }

            // Xác thực mật khẩu
            bool isPasswordValid = CoreHelper.VerifyPassword(model.Password, user.Password);

            if (!isPasswordValid)
            {
                throw new Exception("Mật khẩu không chính xác.");
            }

            // Thiết lập cookie cho tên người dùng
            var cookieOptions = new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(30),
                HttpOnly = true
            };

            // Lưu tên người dùng vào cookie
            _httpContextAccessor.HttpContext.Response.Cookies.Append("UserName", user.UserName, cookieOptions);

            // Lưu id người dùng vào cookie
            _httpContextAccessor.HttpContext.Response.Cookies.Append("UserId", user.Id.ToString(), cookieOptions);
            ApplicationUserRoles? userRole = _unitOfWork.GetRepository<ApplicationUserRoles>().Entities.FirstOrDefault(x=>x.UserId == user.Id);
            ApplicationRole applicationRole = _unitOfWork.GetRepository<ApplicationRole>().Entities.FirstOrDefault(x => x.Id == userRole.RoleId);
            // Lưu quyền người dùng vào cookie
            var role = applicationRole.Name;
            _httpContextAccessor.HttpContext.Response.Cookies.Append("UserRole", role, cookieOptions);

            // Chuyển hướng theo quyền
            return role switch
            {
                "Admin" => "/Admin/Index",
                _ => "/Home/Index"
            };
        }


        public async Task<bool> RegisterAsync(RegisterModel model)
        {

            // Kiểm tra xem người dùng đã tồn tại chưa
            var existingUser = await _unitOfWork.GetRepository<ApplicationUser>().Entities.FirstOrDefaultAsync(u => u.Email.ToLower() == model.Email.ToLower() && !u.DeletedTime.HasValue);

            if (existingUser != null)
            {
                throw new InvalidOperationException("User already exists!");
            }

            // Mã hóa mật khẩu
            string hashedPassword = CoreHelper.HashPassword(model.Password);

            // Tạo người dùng mới
            ApplicationUser newUser = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                Password = hashedPassword,
                Phone = model.Phone,
                CreatedTime = DateTime.UtcNow,
            };
            ApplicationUserRoles newApplicationUserRole = new ApplicationUserRoles
            {
                UserId = newUser.Id,
                RoleId = Guid.Parse(model.RoleId),
                CreatedTime = CoreHelper.SystemTimeNow
            };

            try
            {
                // Thêm người dùng mới vào cơ sở dữ liệu
                await _unitOfWork.GetRepository<ApplicationUser>().InsertAsync(newUser);
                await _unitOfWork.SaveAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while saving the user. Please try again.");
            }

            return true;
        }

    }


}
