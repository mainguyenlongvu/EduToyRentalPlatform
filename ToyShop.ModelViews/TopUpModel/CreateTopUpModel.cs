using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyShop.ModelViews.TopUpModel
{
	public class CreateTopUpModel
	{
		[Required(ErrorMessage = "Phải nhập tên khách hàng")]
		[DisplayName("Tên khách hàng")]
		[StringLength(50, MinimumLength = 10, ErrorMessage = "Chiều dài tối đa chỉ {2} - {1}")]
		public string? CustomerName { get; set; }


		[DataType(DataType.Currency)]
		[Range(100000, double.MaxValue, ErrorMessage = "Giá trị phải lớn hơn 100000.")]
		[DisplayName("Tổng giá trị")]
		public decimal TotalValue { get; set; }   // Total value of the contract (decimal)
	}
}
