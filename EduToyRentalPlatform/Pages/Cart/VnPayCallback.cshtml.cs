using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToyShop.Contract.Services.Interface;
using ToyShop.ModelViews.ContractDetailModelView;
using ToyShop.ModelViews.ContractModelView;
using ToyShop.ModelViews.PaymentModelView;
using ToyShop.ModelViews.TransactionModelView;

namespace EduToyRentalPlatform.Pages.Cart
{
    public class PaymentCallbackModel : PageModel
    {
        private IVnPayService _vnPayService;
        private ITransactionService _transactionService;
        private IContractService _contractService;
        private IHttpContextAccessor _contextAccessor;

        public PaymentCallbackModel(IVnPayService vnPayService, ITransactionService transactionService, IContractService contractService, IHttpContextAccessor contextAccessor)
        {
            _vnPayService = vnPayService;
            _transactionService = transactionService;
            _contractService = contractService;
            _contextAccessor = contextAccessor;
        }

        public async void OnGet()
        {
            foreach (var (key, value) in Request.Query)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    await PaymentCallBack();
                    break;
                }
            }
        }

        public async Task<IActionResult> PaymentCallBack()
        {
            Console.WriteLine("VnPay Callback called");
            var vnPayRes = _vnPayService.PaymentExecute(Request.Query);

            if (vnPayRes == null || !vnPayRes.VnPayResponseCode.Equals("00"))
            {
                return Redirect("TestFailed");
            }
            bool isTopUp = (bool) _contextAccessor.HttpContext?.Session.GetString("OrderType").Equals("TopUp");
            if (isTopUp == null)
                return Redirect("TestFailed");

            bool result;
            if (isTopUp)
                result = await OnTopUpSuccess(vnPayRes);
            else
                result = await OnPurchaseSuccess(vnPayRes);

            if (result)
                return Redirect("TestSuccess");
            else 
                return Redirect("TestFailed");         
        }

        private async Task<bool> OnTopUpSuccess(VnPayResponseModel vnPayRes)
        {
            var tranModel = HttpContext.Session.GetObject<CreateTransactionModel>("TranModel");
            var userId = HttpContext.Session.GetString("UserId");
            var amount = (int) HttpContext.Session.GetInt32("TopUpAmount");
            if (tranModel == null || userId == null || amount <= 0)
                Response.Redirect("TestFailed");

            bool result = await _transactionService.ProcessTopUpVnPay(tranModel, userId, amount);

            return result;
        }

        private async Task<bool> OnPurchaseSuccess(VnPayResponseModel vnPayRes)
        {
            var tranModel = HttpContext.Session.GetObject<CreateTransactionModel>("TranModel");
            var userId = HttpContext.Session.GetString("UserId");
            if (tranModel == null || userId == null)
                Response.Redirect("TestFailed");

            bool result = await _transactionService.ProcessPurchaseVnPay(tranModel, userId);
            return result;
        }

    }
}
