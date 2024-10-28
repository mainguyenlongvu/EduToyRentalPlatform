﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyShop.ModelViews.PaymentModelView
{
	public class VnPayResponseModel
	{
		public bool	Success {  get; set; }
		public string PaymentMethod {  get; set; }
		public string OrderDescription {  get; set; }
		public string OrderId {  get; set; }
		public string PaymentId {  get; set; } 
		public string TransactionId {  get; set; } 
		public string Token {  get; set; } 
		public string VnPayResponseCode {  get; set; } 
	}
}