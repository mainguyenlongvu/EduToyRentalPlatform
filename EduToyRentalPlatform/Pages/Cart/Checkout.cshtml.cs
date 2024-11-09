using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToyShop.Contract.Services.Interface;
using ToyShop.ModelViews.ContractDetailModelView;
using ToyShop.ModelViews.ContractModelView;
using ToyShop.ModelViews.PaymentModelView;
using ToyShop.ModelViews.TransactionModelView;

namespace EduToyRentalPlatform.Pages.Cart
{
    public class CheckoutModel : PageModel
    {
        private IVnPayService _vnPayService;



        public CheckoutModel(IVnPayService vnPayService)
        {
            _vnPayService = vnPayService;
        }


        [BindProperty]
        public bool isTopUp { get; set; }


        [BindProperty]
        public CreateContractModel CreateContractModel { get; set; }

        [BindProperty]
        public CreateContractDetailModel CreateContractDetailModel { get; set; }

        [BindProperty]
        public CreateTransactionModel CreateTransactionModel { get; set; }


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
        public void OnPost() 
        {
            Console.WriteLine("VnPay OnPost Called");
            var model = new VnPayRequestModel()
            {
                OrderType = isTopUp ? "260000" : "190000", // https://sandbox.vnpayment.vn/apis/docs/loai-hang-hoa/
                Amount = Double.Parse(CreateContractModel.TotalValue.ToString()),
                OrderDescription = $"Thanh toan don hang {CreateTransactionModel.TranCode}",
                Name = "EduToyRent thanh toan",
                IpAddress = "127.0.0.1"
            };
            string url = CreatePaymentUrl(model, HttpContext);
            
            Response.Redirect(url);         
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
            Response.Redirect("TestSuccess");
        }
    }
}
