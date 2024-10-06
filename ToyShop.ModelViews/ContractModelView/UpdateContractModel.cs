using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyShop.ModelViews.ContractModelView
{
    public class UpdateContractModel
    {

        [DataType(DataType.Currency)]
        [Range(100000, double.MaxValue, ErrorMessage = "Giá trị phải lớn hơn 100000.")]
        [DisplayName("Tổng giá trị")]
        public decimal  TotalValue { get; set; }   // Total value of the contract (decimal)

        [DataType(DataType.Date)]
        [DisplayName("Ngày bắt đầu")]
        public DateOnly? DateStart { get; set; }   // Start date of the contract (DateTime)

        [DataType(DataType.Date)]
        [DisplayName("Ngày hết hạn")]
        public DateOnly? DateEnd { get; set; }   // End date of the contract (DateTime)

        [DisplayName("Trạng thái")]
        public string?   Status { get; set; }   // Status of the contract (string)
    }
}
