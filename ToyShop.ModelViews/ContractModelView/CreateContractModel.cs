using ToyShop.Core.Store;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ToyShop.Core.Utils;

using static ToyShop.Core.Base.BaseException;
using ToyShop.Core.Constants;
namespace ToyShop.ModelViews.ContractModelView
{
    public class CreateContractModel
    {
        [Required(ErrorMessage = "Phải nhập tên khách hàng")]
        [DisplayName("Tên khách hàng")]
        [StringLength(50,MinimumLength = 10,ErrorMessage = "Chiều dài tối đa chỉ {2} - {1}")]
        public string?   CustomerName { get; set; }
        [Required(ErrorMessage = "Phải nhập chọn đồ chơi")]
        [DisplayName("Tên đồ chơi")]
        public string?   ToyName { get; set; }

        [Required(ErrorMessage = "Phải chọn nhà cung cấp")]
        [DisplayName("Tên nhà cung cấp")]
        public string?   SupplierName { get; set; }

        [Required(ErrorMessage = "Phải chọn nhân viên")]
        [DisplayName("Nhân viên")]
        public string?   StaffConfirmed { get; set; }   // Confirmed by staff (staff ID - integer)

        [DataType(DataType.Currency)]
        [Range(100000, double.MaxValue, ErrorMessage = "Giá trị phải lớn hơn 100000.")]
        [DisplayName("Tổng giá trị")]
        public decimal  TotalValue { get; set; }   // Total value of the contract (decimal)

        [Required(ErrorMessage = "Hãy chọn ngày hiệu lực")]
        [DataType(DataType.Date)]
        [DisplayName("Ngày bắt đầu")]
        public DateTimeOffset? DateStart { get; set; } = CoreHelper.SystemTimeNow;   // Start date of the contract (DateTime)

        [Required(ErrorMessage = "Hãy chọn ngày hết hạn")]
        [DataType(DataType.Date)]
        [DateCompareGreaterThan("DateStart", ErrorMessage = "Ngày hết hạn phải lớn hơn ngày bắt đầu")]
        [DisplayName("Ngày hết hạn")]
        public DateTimeOffset? DateEnd { get; set; }   // End date of the contract (DateTime)

        [Required(ErrorMessage = "Hãy chọn loại hợp đồng")]
        [DisplayName("Loại hợp đồng")]
        public string?   ContractType { get; set; }   // Type of contract (string)

        [DisplayName("Trạng thái")]
        public string?   Status { get; set; }   // Status of the contract (string)

        public void CheckValidate()
        {
            if (string.IsNullOrWhiteSpace(ToyName))
            {
                throw new ErrorException((int)StatusCodeHelper.Notfound, ResponseCodeConstants.INVALID_INPUT, "Phai nhap ten do choi");
            }
            else if (string.IsNullOrWhiteSpace(SupplierName))
            {
                throw new ErrorException((int)StatusCodeHelper.Notfound, ResponseCodeConstants.INVALID_INPUT, "Phai nhap nha cung cap do choi");
            }
            else if (string.IsNullOrWhiteSpace(CustomerName))
            {
                throw new ErrorException((int)StatusCodeHelper.Notfound, ResponseCodeConstants.INVALID_INPUT, "Phai nhap khach hang");
            }
            else if (string.IsNullOrWhiteSpace(StaffConfirmed))
            {
                throw new ErrorException((int)StatusCodeHelper.Notfound, ResponseCodeConstants.INVALID_INPUT, "Phai nhap nhan vien xu ly");
            }
            else if (string.IsNullOrWhiteSpace(ContractType))
            {
                throw new ErrorException((int)StatusCodeHelper.Notfound, ResponseCodeConstants.INVALID_INPUT, "Phai nhap loai hop dong thue/mua");
            }
            else if (string.IsNullOrWhiteSpace(ToyName))
            {
                throw new ErrorException((int)StatusCodeHelper.Notfound, ResponseCodeConstants.INVALID_INPUT, "Phai nhap ten do choi");
            }
            else if (DateEnd == null)
            {
                throw new ErrorException((int)StatusCodeHelper.Notfound, ResponseCodeConstants.INVALID_INPUT, "Chon ngay ket thuc");
            }
        }
    }
}
