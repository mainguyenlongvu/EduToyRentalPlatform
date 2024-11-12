using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using ToyShop.Contract.Services.Interface;
using ToyShop.ModelViews.PaymentModelView;
using ToyShop.Services.Service;

namespace EduToyRentalPlatform.Pages.Cart
{
    public class VnPayTestModel : PageModel
    {

        private IVnPayService _vnPayService;

        public VnPayTestModel(IVnPayService vnpay)
        {
            _vnPayService = vnpay;
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

        [ValidateAntiForgeryToken]
        public IActionResult OnPost()
        {
            Console.WriteLine("OnPost Called");
            var model = new VnPayRequestModel()
            {
                OrderType = "110000",
                Amount = 1000,
                OrderDescription = "Test thanh toan vnpay",
                Name = "Do Choi",
                IpAddress = "127.0.0.1"
            };
            string url = CreatePaymentUrl(model, HttpContext);
            return Redirect(url);
        }

        private string CreatePaymentUrl(VnPayRequestModel model, HttpContext context)
        {
            string url = _vnPayService.CreatePaymentUrl(model, context);
            return url;
        }

        
        
        public void PaymentCallBack()
        {
            Console.WriteLine("VnPay Callback called");
            var response = _vnPayService.PaymentExecute(Request.Query);
            
            if (response == null || !response.VnPayResponseCode.Equals("00"))
            {
                Response.Redirect("TestFailed");
                return;
            }
            Console.WriteLine(response.ToString);
            Response.Redirect("TestSuccess");
        }
    }
}
