using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyShop.ModelViews.ContractModelView
{
    public class ResponseContractModel
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
        public DateOnly? DateStart { get; set; }   // Start date of the contract (DateTime)

        [Required(ErrorMessage = "Hãy chọn ngày hết hạn")]
        [DataType(DataType.Date)]
        [DisplayName("Ngày hết hạn")]
        public DateOnly? DateEnd { get; set; }   // End date of the contract (DateTime)

        [Required(ErrorMessage = "Hãy chọn loại hợp đồng")]
        [DisplayName("Loại hợp đồng")]
        public string?   ContractType { get; set; }   // Type of contract (string)

        [DisplayName("Trạng thái")]
        public string?   Status { get; set; }   // Status of the contract (string)
    }
}
