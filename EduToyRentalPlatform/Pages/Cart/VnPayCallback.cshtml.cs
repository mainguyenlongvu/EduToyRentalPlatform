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

        public PaymentCallbackModel(IVnPayService vnPayService, ITransactionService transactionService, IContractService contractService)
        {
            _vnPayService = vnPayService;
            _transactionService = transactionService;
            _contractService = contractService;
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

        public async Task PaymentCallBack()
        {
            Console.WriteLine("VnPay Callback called");
            var vnPayRes = _vnPayService.PaymentExecute(Request.Query);

            if (vnPayRes == null || !vnPayRes.VnPayResponseCode.Equals("00"))
            {
                Response.Redirect("Cart/TestFailed");
                return;
            }

            if (Request.Query.FirstOrDefault(m => m.Key.Equals("vnp_OrderInfo")).Value.Contains("nap vi"))
                await OnTopUpSuccess(vnPayRes);
            else
                await OnPurchaseSuccess(vnPayRes);

            Response.Redirect("Cart/TestSuccess");
        }

        private async Task OnTopUpSuccess(VnPayResponseModel vnPayRes)
        {
            var tranModel = HttpContext.Session.GetObject<CreateTransactionModel>("CreateTransactionModel");
            var userId = HttpContext.Session.GetString("UserId");
            if (tranModel == null || userId == null)
                Response.Redirect("Cart/TestFailed");

            bool result = await _transactionService.ProcessTopUp(tranModel, userId);
            if (!result)
                Response.Redirect("Cart/TestFailed");
        }

        private async Task OnPurchaseSuccess(VnPayResponseModel vnPayRes)
        {
            var tranModel = HttpContext.Session.GetObject<CreateTransactionModel>("CreateTransactionModel");
            var userId = HttpContext.Session.GetString("UserId");
            if (tranModel == null || userId == null)
                Response.Redirect("Cart/TestFailed");

            bool result = await _transactionService.ProcessPurchaseVnPay(tranModel, userId);
            if (!result)
                Response.Redirect("Cart/TestFailed");
        }

    }
}
