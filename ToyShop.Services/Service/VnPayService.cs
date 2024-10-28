﻿using Microsoft.AspNetCore.Http;
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

			return paymentUrl;
		}

		public VnPayResponseModel PaymentExecute(IQueryCollection collections)
		{
			var pay = new VnPayLibrary();

			foreach (var (key, value) in collections)
			{
				if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
				{
					pay.AddResponseData(key, value);
				}
			}

			var orderId = Convert.ToInt64(pay.GetResponseData("vnp_TxnRef"));
			var vnPayTranId = Convert.ToInt64(pay.GetResponseData("vnp_TransactionNo"));
			var vnpResponseCode = pay.GetResponseData("vnp_ResponseCode");
			var vnpSecureHash = collections.FirstOrDefault(k => k.Key == "vnp_SecureHash").Value; //hash của dữ liệu trả về
			var orderInfo = pay.GetResponseData("vnp_OrderInfo");
			var checkSignature = pay.ValidateSignature(vnpSecureHash, _configuration["Vnpay:HashSecret"]); //check Signature

			if (!checkSignature)
				return new VnPayResponseModel()
				{
					Success = false
				};

			return new VnPayResponseModel()
			{
				Success = true,
				PaymentMethod = "VnPay",
				OrderDescription = orderInfo,
				OrderId = orderId.ToString(),
				PaymentId = vnPayTranId.ToString(),
				TransactionId = vnPayTranId.ToString(),
				Token = vnpSecureHash,
				VnPayResponseCode = vnpResponseCode
			};
		}
	}
}