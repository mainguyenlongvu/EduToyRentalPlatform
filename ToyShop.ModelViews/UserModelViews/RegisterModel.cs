using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ToyShop.ModelViews.UserModelViews
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Tên tài khoản là bắt buộc.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Số điện thoại là bắt buộc.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Vai trò là bắt buộc.")]
        public string RoleId { get; set; }
    }

}
