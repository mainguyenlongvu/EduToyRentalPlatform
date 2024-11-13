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
                Response.Redirect("TestFailed");
                return;
            }

            if (Request.Query.FirstOrDefault(m => m.Key.Equals("vnp_OrderInfo")).Value.Contains("nap vi"))
                await OnTopUpSuccess(vnPayRes);
            else
                await OnPurchaseSuccess(vnPayRes);

            Response.Redirect("TestSuccess");
        }

        private async Task OnTopUpSuccess(VnPayResponseModel vnPayRes)
        {
            var tranModel = HttpContext.Session.GetObject<CreateTransactionModel>("CreateTransactionModel");
            var updateContract = new UpdateContractModel()
            {
                Status = "Done"
            };
            await _contractService.UpdateContractAsync(tranModel.ContractId, updateContract);
            await _transactionService.Insert(tranModel);
        }

        private async Task OnPurchaseSuccess(VnPayResponseModel vnPayRes)
        {
            var tranModel = HttpContext.Session.GetObject<CreateTransactionModel>("CreateTransactionModel");
            var updateContract = new UpdateContractModel()
            {
                Status = "Done"
            };
            await _contractService.UpdateContractAsync(tranModel.ContractId, updateContract);
            await _transactionService.Insert(tranModel);
        }

    }
}
