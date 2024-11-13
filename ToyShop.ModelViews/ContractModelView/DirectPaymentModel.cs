using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyShop.ModelViews.ContractModelView
{
    public class DirectPaymentModel
    {
        [DataType(DataType.Currency)]
        [Range(100000, double.MaxValue, ErrorMessage = "Giá trị phải lớn hơn 100000.")]
        [DisplayName("Tổng giá trị")]
        public decimal TotalValue { get; set; }   // Total value of the contract (decimal)

        [DataType(DataType.Currency)]
        [DisplayName("Số lượng đồ chơi thuê")]
        public int? NumberOfRentals { get; set; }

        [DataType(DataType.Text)]
        [DisplayName("Ghi chú")]
        public string? Note { get; set; }

    }
}
