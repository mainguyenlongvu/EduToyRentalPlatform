using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyShop.ModelViews.GmailModel
{
	public class EmailRequestDto
	{

		public string ReceiverEmail { get; set; }
		public string EmailSubject { get; set; }
		public string EmailBody { get; set; }
		public bool IsHtml { get; set; }
	}
}
