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
using Microsoft.Extensions.Options;
using ToyShop.Core.Constants;
using Castle.Core.Resource;
using ToyShop.Contract.Repositories.PaggingItems;
using Azure.Core;
using ToyShop.Core.Base;
using System.Text.RegularExpressions;
using Microsoft.AspNet.Identity;
using ToyShop.ModelViews.GmailModel;


namespace ToyShop.Services.Service
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly GmailService _gmailService;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly IConfiguration _configuration;
        private const string customer = "Customer";

        public UserService(IHttpContextAccessor _httpContextAccessor1, IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration, IPasswordHasher<ApplicationUser> passwordHasher, GmailService gmailService)
        {
            _httpContextAccessor = _httpContextAccessor1;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
            _gmailService = gmailService;
        }
        public async Task ChangPasswordAsync(ChangPasswordModel model)
        {
            // Mã hóa mật khẩu
            string hashedPassword = CoreHelper.HashPassword(model.Password);
            //kiểm tra user có tồn tại
            var user = await _unitOfWork.GetRepository<ApplicationUser>().Entities
                         .FirstOrDefaultAsync(u => (u.Email == model.UserName || u.UserName == model.UserName) && u.Password == model.Password && !u.DeletedTime.HasValue) ?? throw new Exception("Người dùng không tồn tại.");
            //Điều kiện
            if (model.NewPassword != model.ConfirmPassword)
            {
                throw new Exception("Vui lòng điền mật khẩu và xác nhận mật khẩu giống nhau");
            }
            if (model.NewPassword.Length < 6)
            {
                throw new Exception("Vui lòng điền mật khẩu dài hơn 6 kí tự");
            }
            //Gán vào user
            user.Password = model.NewPassword;
            user.PasswordHash = hashedPassword;

            List<string> selectedEmail = new List<string> { user.Email!.ToLower() };
            string body = $"<p>Mật khẩu của bạn đổi thàng công vào lúc: <strong>{CoreHelper.SystemTimeNow}</strong></p>";
            //gửi email xác thực tài khoản
            //await _emailService.SendEmailAsync(selectedEmail, "Đổi mật khẩu tài khoản", body);
            //Cập nhật vào Db
            await _unitOfWork.GetRepository<ApplicationUser>().UpdateAsync(user);
            await _unitOfWork.SaveAsync();

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
        public async Task<BasePaginatedList<ApplicationUser>> GetPageAsync(int index, int pageSize, string nameSearch)
        {
            // Check if the index is valid
            if (index <= 0)
            {
                throw new Exception("Vui lòng nhập index lớn hơn 0");
            }

            // Check if the pageSize is valid
            if (pageSize <= 0)
            {
                throw new Exception("Vui lòng nhập pageSize lớn hơn 0");
            }

            // Get UserId from cookies and convert to uppercase
            string userId = _httpContextAccessor.HttpContext?.Request.Cookies["UserId"]!;
            string idUser = userId.ToUpper();

            // Convert to Guid
            Guid.TryParse(idUser, out Guid guid);

            // Assume we are fetching an IQueryable of ApplicationUser
            IQueryable<ApplicationUser> query = _unitOfWork.GetRepository<ApplicationUser>().Entities;

            // Get the "Customer" role
            ApplicationRole? customerRole = await _unitOfWork.GetRepository<ApplicationRole>().Entities
                                                         .Where(r => r.Name == "Customer" && !r.DeletedTime.HasValue)
                                                         .FirstOrDefaultAsync();

            // Filter by role "Customer"
            if (customerRole != null)
            {
                List<Guid> userRoles = await _unitOfWork.GetRepository<ApplicationUserRoles>().Entities
                                                        .Where(ur => ur.RoleId == customerRole.Id && !ur.DeletedTime.HasValue)
                                                        .Select(ur => ur.UserId)
                                                        .ToListAsync();

                // Only include users with "Customer" role
                query = query.Where(lp => userRoles.Contains(lp.Id));
            }

            // Search by full name if the nameSearch is provided
            if (!string.IsNullOrWhiteSpace(nameSearch))
            {
                query = query.Where(lp => lp.FullName.Contains(nameSearch));
            }

            // Filter out users that are deleted and those whose email is not confirmed
            query = query.Where(x => !x.DeletedTime.HasValue && x.EmailConfirmed == true && x.Id != guid);

            // Order by full name
            query = query.OrderBy(e => e.FullName);

            // Paginate the query results
            BasePaginatedList<ApplicationUser> resultQuery = await _unitOfWork.GetRepository<ApplicationUser>().GetPagging(query, index, pageSize);

            // Return the paginated results
            return resultQuery;
        }
        public async Task<string> LoginAsync(LoginModel model)
        {

            // Tìm người dùng trong cơ sở dữ liệu
            var user = await _unitOfWork.GetRepository<ApplicationUser>().Entities
                .FirstOrDefaultAsync(u => (u.Email == model.Email || u.UserName == model.Email)&& u.Password == model.Password && !u.DeletedTime.HasValue) ?? throw new Exception("Người dùng không tồn tại.");


            // Xác thực mật khẩu
            string passwordValid = CoreHelper.HashPassword(model.Password);

            if (user.PasswordHash == passwordValid)
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
            ApplicationUserRoles? userRole = _unitOfWork.GetRepository<ApplicationUserRoles>().Entities.FirstOrDefault(x => x.UserId == user.Id && !x.DeletedTime.HasValue);
            ApplicationRole applicationRole = _unitOfWork.GetRepository<ApplicationRole>().Entities.FirstOrDefault(x => x.Id == userRole.RoleId);
            // Lưu quyền người dùng vào cookie
            var role = applicationRole.Name;
            _httpContextAccessor.HttpContext.Response.Cookies.Append("UserRole", role, cookieOptions);

            // Chuyển hướng theo quyền
            return role switch
            {
                "Admin" => "/Admin/Index",
                _ => "/Shop"
            };
        }
        public async Task<bool> RegisterAsync(RegisterModel model)
        {
            // Kiểm tra xem người dùng đã tồn tại chưa
            var existingUser = await _unitOfWork.GetRepository<ApplicationUser>().Entities
                .FirstOrDefaultAsync(u => (u.Email == model.Email || u.UserName == model.UserName) && !u.DeletedTime.HasValue);

            if (existingUser != null)
            {
                throw new InvalidOperationException("User already exists!");
            }

            // Tìm Role
            var role = await _unitOfWork.GetRepository<ApplicationRole>().Entities
                .FirstOrDefaultAsync(x => x.Name == model.RoleName);

            // Mã hóa mật khẩu
            string hashedPassword = CoreHelper.HashPassword(model.Password);

            // Tạo người dùng mới
            ApplicationUser newUser = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                PasswordHash = hashedPassword, // Gán mật khẩu đã mã hóa
                Phone = model.Phone,
                FullName = model.FullName,
                CreatedTime = DateTime.UtcNow,
                ImageUrl = await FileUploadHelper.UploadFile(model.Image),
                Password = model.Password
            };

            try
            {
                // Thêm người dùng mới vào cơ sở dữ liệu
                await _unitOfWork.GetRepository<ApplicationUser>().InsertAsync(newUser);
                await _unitOfWork.SaveAsync(); // Lưu thay đổi trước để gán ID

                // Tạo vai trò cho người dùng mới
                ApplicationUserRoles newApplicationUserRole = new ApplicationUserRoles
                {
                    UserId = newUser.Id, // Gán UserId sau khi người dùng được lưu
                    RoleId = role.Id,
                    CreatedTime = CoreHelper.SystemTimeNow
                };

                // Thêm vai trò vào cơ sở dữ liệu
                await _unitOfWork.GetRepository<ApplicationUserRoles>().InsertAsync(newApplicationUserRole);
                await _unitOfWork.SaveAsync(); // Lưu thay đổi
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while saving the user. Please try again.");
            }

            return true;
        }
        public async Task ChangPasswordAdminAsync(ChangPasswordAdminModel model)
        {
            //
            string passwordValid = CoreHelper.HashPassword(model.NewPassword);

            //kiểm tra user có tồn tại
            ApplicationUser? user = await _unitOfWork.GetRepository<ApplicationUser>().Entities.Where(x => x.DeletedTime == null && x.Id == model.Id).FirstOrDefaultAsync() ??
                throw new Exception("Không tìm thấy tài khoản");
            //Gán vào user
            user.Password = model.NewPassword;
            user.PasswordHash = passwordValid;
            //Cập nhật vào Db
            await _unitOfWork.GetRepository<ApplicationUser>().UpdateAsync(user);
            await _unitOfWork.SaveAsync();

        }
        //public async Task CreateCustomer(CreateCustomerModel model)
        //{
        //    if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
        //    {
        //        throw new ErrorException(StatusCodes.Status406NotAcceptable, ResponseCodeConstants.INVALID_INPUT, "Vui lòng điền tên đăng nhập và mật khẩu");
        //    }
        //    ApplicationUser? existingUser = await _unitOfWork.GetRepository<ApplicationUser>().Entities.Where(x => x.UserName == model.Username && x.DeletedTime == null).FirstOrDefaultAsync();

        //    // kiểm tra nếu người dùng tồn tại và không bị xóa mềm
        //    if (existingUser != null)
        //    {
        //        throw new Exception("Tên đăng nhập đã tồn tại");
        //    }
        //    //Kiểm tra role
        //    ApplicationRole existedRole = await _unitOfWork.GetRepository<ApplicationRole>().Entities.Where(x => x.Name == "Customer").FirstOrDefaultAsync() ??
        //        throw new Exception("Vai trò không tồn tại");
        //    //kiểm tra email
        //    ApplicationUser? existingEmail = await _unitOfWork.GetRepository<ApplicationUser>().Entities.Where(x => x.Email == model.Email!.ToLower() && x.DeletedTime == null).FirstOrDefaultAsync();

        //    // kiểm tra nếu email tồn tại và không bị xóa mềm
        //    if (existingEmail != null)
        //    {
        //        throw new Exception("Email đã tồn tại");
        //    }
        //    //kiểm tra sđt
        //    ApplicationUser? existingPhone = await _unitOfWork.GetRepository<ApplicationUser>().Entities.Where(x => x.PhoneNumber == model.PhoneNumber && x.DeletedTime == null).FirstOrDefaultAsync();
        //    // kiểm tra nếu email tồn tại và không bị xóa mềm
        //    if (existingPhone != null)
        //    {
        //        throw new Exception("Số điện thoại đã tồn tại");
        //    }
        //    //kiểm tra số đt
        //    if (!IsValidPhoneNumber(model.PhoneNumber!))
        //    {
        //        throw new ErrorException(StatusCodes.Status406NotAcceptable, ResponseCodeConstants.INVALID_INPUT, "Số điện thoại không hợp lệ.");

        //    }
        //    // Kiểm tra mật khẩu không chứa khoảng trắng và ký tự có dấu
        //    if (string.IsNullOrWhiteSpace(model.Password)
        //        || model.Password.Any(char.IsWhiteSpace)
        //        || model.Password.Any(c => char.IsLetter(c) && c > 127))
        //    {
        //        throw new ErrorException(StatusCodes.Status406NotAcceptable, ResponseCodeConstants.INVALID_INPUT, "Mật khẩu không hợp lệ");
        //    }

        //    ApplicationUser user = new ApplicationUser
        //    {
        //        Id = Guid.NewGuid(),
        //        UserName = model.Username,
        //        Email = model.Email!.ToLower(),
        //        FullName = model.FullName!,
        //        Password = model.Password,
        //        PhoneNumber = model.PhoneNumber,
        //        SecurityStamp = Guid.NewGuid().ToString(),
        //    };
        //    // Tạo mã code ngẫu nhiên và hash nó
        //    string code = new Random().Next(1000, 9999).ToString();

        //    //gán vào user
        //    //user.EmailCode = code;
        //    List<string> selectedEmail = new List<string> { model.Email.ToLower() };
        //    string body = $"<p>Code kích hoạt tài khoản của bạn là: <strong>{code}</strong></p>";
        //    //gửi email xác thực tài khoản
        //    await _emailService.SendEmailAsync(selectedEmail, "Code kích hoạt tài khoản", body);


        //    FixedSaltPasswordHasher<ApplicationUser> passwordHasher = new FixedSaltPasswordHasher<ApplicationUser>(Options.Create(new PasswordHasherOptions()));

        //    //var hashedInputPassword = HashPasswordService.HashPasswordTwice(request.Password);
        //    user.PasswordHash = passwordHasher.HashPassword(user, model.Password);

        //    //var roleResult = await _userManager.AddToRoleAsync(user, request.Role);
        //    ApplicationUserRoles userRole = new ApplicationUserRoles()
        //    {
        //        UserId = user.Id,
        //        RoleId = existedRole.Id,
        //    }
        //    await _unitOfWork.GetRepository<ApplicationUserRoles>().InsertAsync(userRole);
        //    await _unitOfWork.GetRepository<ApplicationUser>().InsertAsync(user);
        //    await _unitOfWork.SaveAsync();
        //}
        static bool IsValidPhoneNumber(string phoneNumber)
        {
            // Mẫu regex để kiểm tra số điện thoại, bạn có thể tùy chỉnh theo yêu cầu cụ thể
            string pattern = @"^(\+84|0)([3|5|7|8|9])+([0-9]{8})$";

            // Sử dụng Regex để kiểm tra
            Regex regex = new Regex(pattern);
            return regex.IsMatch(phoneNumber);
        }
        public async Task UpdateCustomerAsync(Guid id, UpdateCustomerModel model)
        {
            //Tìm người dùng
            ApplicationUser? existingUser = await _unitOfWork.GetRepository<ApplicationUser>().Entities.Where(x => x.Id == id && x.DeletedTime == null).FirstOrDefaultAsync() ??
                throw new Exception("Người dùng không tồn tại");

            //khi thay đổi mới đk
            if (existingUser.PhoneNumber != model.Phone)
            {
                //kiểm tra sđt
                ApplicationUser? existingPhone = await _unitOfWork.GetRepository<ApplicationUser>().Entities.Where(x => x.PhoneNumber == model.Phone && x.DeletedTime == null).FirstOrDefaultAsync();
                // kiểm tra nếu email tồn tại và không bị xóa mềm
                if (existingPhone != null)
                {
                    throw new Exception("Số điện thoại đã tồn tại");
                }
            }

            //kiểm tra số đt
            if (!IsValidPhoneNumber(model.Phone))
            {
                throw new Exception("Số điện thoại không hợp lệ.");

            }
            // Tạo mã code ngẫu nhiên và hash nó
            string code = new Random().Next(1000, 9999).ToString();
            //string body = $"<p>Code kích hoạt tài khoản của bạn là:: <strong>{code}</strong></p>";
            ////gửi email xác thực tài khoản
            ////await _emailService.SendEmailAsync(selectedEmail, "Code kích hoạt tài khoản", body);
            ////kiểm tra xem phone có trống không
            if (string.IsNullOrWhiteSpace(model.Phone))
            {
                throw new Exception("Không để số điện thoại trống");
            }
            //kiểm tra xem name có trống không
            if (string.IsNullOrWhiteSpace(model.FullName))
            {
                throw new Exception("Không để số tên trống");
            }
            //Gán vào user
            existingUser.FullName = model.FullName;
            existingUser.EmailConfirmed = false;
            existingUser.LastUpdatedTime = CoreHelper.SystemTimeNow;
            //existingUser.EmailCode = code.ToString();
            existingUser.Phone = model.Phone;
            existingUser.FullName = model.FullName;

            //Lưu thay đổi
            await _unitOfWork.GetRepository<ApplicationUser>().UpdateAsync(existingUser);
            await _unitOfWork.SaveAsync();
        }
        public async Task<bool> DeleteUserAsync(string id)
        {
          
            ApplicationUser user = await GetUserByIdAsync(id);

            // If the user is not found, throw an exception
            if (user == null || user.DeletedTime != null)
            {
                throw new Exception("User not found or already deleted.");
            }

            // Set the DeletedTime to mark as deleted
            user.DeletedTime = CoreHelper.SystemTimeNow;

            // Update the user in the database
            await _unitOfWork.GetRepository<ApplicationUser>().UpdateAsync(user);

            // Save the changes
            await _unitOfWork.SaveAsync();

            // Return true to indicate the deletion was successful
            return true;
        }
        public Task<ApplicationUser> GetUserAsync(LoginModel model)
        {
            throw new NotImplementedException();
        }
        public async Task ForgotPassword(string email)
        {
            // Tìm người dùng qua email
            ApplicationUser user = await _unitOfWork.GetRepository<ApplicationUser>().Entities.FirstOrDefaultAsync(x=>x.Email == email )
                ?? throw new Exception("Không tìm thấy tài khoản");

            // Tạo mã code ngẫu nhiên và hash nó
            string code = new Random().Next(100000, 999999).ToString();
            //var codeHash = _userManager.PasswordHasher.HashPassword(user, code);

            // Cập nhật mã code và thời gian tạo mã cho người dùng
            user.Password = code;
            user.PasswordHash = CoreHelper.HashPassword(code);
            //user.CodeGeneratedTime = DateTime.UtcNow;
            await _unitOfWork.GetRepository<ApplicationUser>().UpdateAsync(user);

            // Gửi email chứa mã code tới người dùng
            //List<string> selectedEmail = new List<string> { email };
            string body = $"<div style=\"font-family: Helvetica,Arial,sans-serif;min-width:1000px;overflow:auto;line-height:2\">\r\n  <div style=\"margin:50px auto;width:70%;padding:20px 0\">\r\n    <div style=\"border-bottom:1px solid #eee\">\r\n      <a href=\"\" style=\"font-size:1.4em;color: #ee0000;text-decoration:none;font-weight:600\">EduToyRent Platform</a>\r\n    </div>\r\n    <p style=\"font-size:1.1em\">Chào bạn,</p>\r\n    <p>Đây là mật khẩu mới của bạn. Vui lòng đổi sau khi đăng nhập.</p>\r\n    <h2 style=\"background: #aa0000;margin: 0 auto;width: max-content;padding: 0 10px;color: #fff;border-radius: 4px;\">@{code}</h2>\r\n    <p style=\"font-size:0.9em;\">Thân,<br />EduToyRent Staff</p>\r\n    <hr style=\"border:none;border-top:1px solid #eee\" />\r\n    <div style=\"float:right;padding:8px 0;color:#aaa;font-size:0.8em;line-height:1;font-weight:300\">\r\n      <p>EduToyRent Platform</p>\r\n      <p>Ho Chi Minh City</p>\r\n      <p>Vietnam</p>\r\n    </div>\r\n  </div>\r\n</div>";
            EmailRequestModel emailRequestModel = new EmailRequestModel
            {
                EmailBody = body,
                IsHtml = true,
                EmailSubject = "Quên mật khẩu",
                ReceiverEmail = email,
            };
            //Kiểm tra gửi email có thành công không
            if (!_gmailService.SendEmailSingle(emailRequestModel))
            {
                throw new Exception("Gửi email thất bại");
            }

        }
    }
}
