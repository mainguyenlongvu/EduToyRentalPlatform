using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyShop.ModelViews.TransactionModelView
{
	public class ResponseTransactionModel
	{
		public string Id { get; set; } = null!;
		public int TranCode { get; set; }
		public DateTime CreatedTime { get; set; }
		public string? Status { get; set; }
		public bool Method { get; set; }
		public string? ContractId { get; set; }
	}
}
