using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToyShop.Contract.Services.Interface;
using ToyShop.ModelViews.PaymentModelView;

namespace EduToyRentalPlatform.Pages.Cart
{
    public class PaymentCallbackModel : PageModel
    {
        private IVnPayService _vnPayService;

        public PaymentCallbackModel(IVnPayService vnPayService)
        {
            _vnPayService = vnPayService;
        }

        public void OnGet()
        {
            foreach (var (key, value) in Request.Query)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    PaymentCallBack();
                    break;
                }
            }
        }


        public void PaymentCallBack()
        {
            Console.WriteLine("VnPay Callback called");
            var vnPayRes = _vnPayService.PaymentExecute(Request.Query);

            if (vnPayRes == null || !vnPayRes.VnPayResponseCode.Equals("00"))
            {
                Response.Redirect("TestFailed");
                return;
            }

            if (Request.Query.FirstOrDefault(m => m.Key.Equals("vnp_OrderInfo")).Value.Contains("nap vi"))
                OnTopUpSuccess(vnPayRes);
            else
                OnPurchaseSuccess(vnPayRes);

            Response.Redirect("TestSuccess");
        }

        private void OnTopUpSuccess(VnPayResponseModel vnPayRes)
        {
            string order = vnPayRes.OrderId;
        }

        private void OnPurchaseSuccess(VnPayResponseModel vnPayRes)
        {
            string order = vnPayRes.OrderId;
        }

    }
}
