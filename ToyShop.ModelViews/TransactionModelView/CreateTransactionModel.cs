using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyShop.ModelViews.TransactionModelView
{
	public class CreateTransactionModel

	{
		public int TranCode { get; set; }
		public bool Method { get; set; }
		public string? Status { get; set; }
		public string? ContractId { get; set; }

	}
}
