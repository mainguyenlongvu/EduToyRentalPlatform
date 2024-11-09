using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToyShop.Contract.Services.Interface;
using ToyShop.ModelViews.PaymentModelView;

namespace ToyShop.Services.Service
{
	public class VnPayService : IVnPayService
	{
		private IConfiguration _configuration;

        public VnPayService(IConfiguration config)
        {
			_configuration = config;
        }

        public string CreatePaymentUrl(VnPayRequestModel model, HttpContext context)
		{
			var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
			var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
			var tick = DateTime.Now.Ticks.ToString();
			var pay = new VnPayLibrary();
			var urlCallBack = _configuration["Vnpay:CallbackUrl"];

			pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
			pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
			pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
			pay.AddRequestData("vnp_Amount", ((int)model.Amount * 100).ToString());
			pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
			pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
			pay.AddRequestData("vnp_IpAddr", model.IpAddress);
			pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
			pay.AddRequestData("vnp_OrderInfo", $"{model.Name} {model.OrderDescription} {model.Amount}");
			pay.AddRequestData("vnp_OrderType", model.OrderType);
			pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
			pay.AddRequestData("vnp_TxnRef", tick);

			var paymentUrl =
				pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);

            Console.WriteLine("version: " + _configuration["Vnpay:Version"]);
			Console.WriteLine("Command: " + _configuration["Vnpay:Command"]);
            Console.WriteLine("TmnCode: "+ _configuration["Vnpay:TmnCode"]);



			return paymentUrl;
		}

		public VnPayResponseModel PaymentExecute(IQueryCollection collections)
		{
			var pay = new VnPayLibrary();

			var response = pay.GetFullResponseData(collections, _configuration["Vnpay:HashSecret"]);

			return response;
		}
	}
}
