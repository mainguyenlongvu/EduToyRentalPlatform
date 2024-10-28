using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToyShop.ModelViews.PaymentModelView;

namespace ToyShop.Contract.Services.Interface
{
	public interface IVnPayService
	{
		string CreatePaymentUrl(VnPayRequestModel model, HttpContext context);
		VnPayResponseModel PaymentExecute(IQueryCollection collections);
	}
}
