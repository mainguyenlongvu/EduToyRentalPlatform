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

namespace ToyShop.Services.Service
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly IConfiguration _configuration;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration, IPasswordHasher<ApplicationUser> passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
        }

        public async Task<string> LoginAsync(LoginModel model)
        {
            var user = await _unitOfWork.GetRepository<ApplicationUser>().Entities
             .FirstOrDefaultAsync(u => u.Email.Equals(model.Email, StringComparison.OrdinalIgnoreCase) && !u.DeletedTime.HasValue);
            if (user == null || _passwordHasher.VerifyHashedPassword(user, user.Password, model.Password) != PasswordVerificationResult.Success)
            {
                throw new Exception("Invalid email or password.");
            }

            var roleUser = _unitOfWork.GetRepository<ApplicationUserRoles>().Entities.FirstOrDefault(x=>x.UserId == user.Id);
            var role = _unitOfWork.GetRepository<ApplicationRole>().Entities.FirstOrDefault(x => x.Id == roleUser.RoleId);
            // Tạo token (nếu cần)
            var token = GenerateToken(user); // gọi một phương thức để tạo token ở đây

            // Điều hướng dựa trên vai trò
            if (role.Name == "Admin")
            {
                return "/Admin/Index";
            }
            else
            {
                return "/Home/Index";
            }
        }

        private object GenerateToken(object user)
        {
            throw new NotImplementedException();
        }

        // Phương thức kiểm tra mật khẩu
        private bool VerifyPassword(string hashedPassword, string password)
        {
            // Sử dụng phương thức kiểm tra mật khẩu của bạn
            return _passwordHasher.VerifyHashedPassword(new ApplicationUser(), hashedPassword, password) == PasswordVerificationResult.Success;
        }

        public async Task<bool> RegisterAsync(RegisterModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.UserName))
                {
                    throw new Exception("Please enter user name!");
                }

                if (string.IsNullOrWhiteSpace(model.Password))
                {
                    throw new Exception("Please enter password!");
                }

                // Kiểm tra xem người dùng đã tồn tại chưa
                var existingUser = await _unitOfWork.GetRepository<ApplicationUser>().Entities
                    .FirstOrDefaultAsync(u => u.UserName.Equals(model.UserName, StringComparison.OrdinalIgnoreCase) && u.DeletedTime == null);

                if (existingUser != null)
                {
                    throw new Exception("User already exists!");
                }

                // Mã hóa mật khẩu
                string hashedPassword = CoreHelper.HashPassword(model.Password);

                // Tạo người dùng mới
                ApplicationUser newUser = _mapper.Map<ApplicationUser>(model);
                newUser.Password = hashedPassword;
                newUser.CreatedTime = DateTime.UtcNow;

                // Thêm người dùng mới vào cơ sở dữ liệu
                await _unitOfWork.GetRepository<ApplicationUser>().InsertAsync(newUser);
                await _unitOfWork.SaveAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while registering the user: {ex.Message}");
            }
        }
    }
}
