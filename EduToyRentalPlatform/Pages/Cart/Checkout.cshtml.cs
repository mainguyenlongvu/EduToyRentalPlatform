using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics.Contracts;
using ToyShop.Contract.Services.Interface;
using ToyShop.ModelViews.ContractDetailModelView;
using ToyShop.ModelViews.ContractModelView;
using ToyShop.ModelViews.PaymentModelView;
using ToyShop.ModelViews.TopUpModel;
using ToyShop.ModelViews.TransactionModelView;

namespace EduToyRentalPlatform.Pages.Cart
{
    public class CheckoutModel : PageModel
    {
        private IVnPayService _vnPayService;
        private IContractService _contractService;  
        private ITransactionService _transactionService;


        #region constructors

        public CheckoutModel(IVnPayService vnPayService, IContractService contractService, ITransactionService transactionService)
        {
            _vnPayService = vnPayService;
            _contractService = contractService;
            _transactionService = transactionService;
        }

        #endregion

        public CreateTransactionModel CreateTransactionModel { get; set; }


        public void OnGet()
        {
        
        }

        [ValidateAntiForgeryToken]
        public async Task OnPost() 
        {
            Console.WriteLine("Payment OnPost Called");

            if (!ModelState.IsValid) 
            { 
                return;
            }
            this.CreateTransactionModel.TranCode = 
                int.Parse(new Random().NextInt64(100000000, 999999999).ToString());

            var contract = await _contractService.GetContractAsync(CreateTransactionModel.ContractId);

            if (CreateTransactionModel.Method) //vnpay
            {
                HttpContext.Session.SetString("TranCode", CreateTransactionModel.TranCode.ToString());
                HttpContext.Session.SetString("ContractId", CreateTransactionModel.ContractId.ToString());

                var model = contract.ToyName == null ?
                    await CreateVnPayTopUpRequest(contract) 
                    : 
                    await CreateVnPayPurchaseRequest(contract);

                string url = CreatePaymentUrl(model, HttpContext);
                Response.Redirect(url);
            }     
        }



        private string CreatePaymentUrl(VnPayRequestModel model, HttpContext context)
        {
            string url = _vnPayService.CreatePaymentUrl(model, context);
            return url;
        }

        private async Task<VnPayRequestModel> CreateVnPayTopUpRequest(ResponseContractModel contract)
        {
            

            var model = new VnPayRequestModel()
            {
                OrderType = "260000", // https://sandbox.vnpayment.vn/apis/docs/loai-hang-hoa/
                Amount = Double.Parse(contract.TotalValue.ToString()),
                OrderDescription = $"Thanh toan nap vi {CreateTransactionModel.ContractId}",
                Name = contract.CustomerName == null ? "EduToyRent" : contract.CustomerName,
                IpAddress = "127.0.0.1"
            };

            return model;
        }

        private async Task<VnPayRequestModel> CreateVnPayPurchaseRequest(ResponseContractModel contract)
        {

            var model = new VnPayRequestModel()
            {
                OrderType = "190000", // https://sandbox.vnpayment.vn/apis/docs/loai-hang-hoa/
                Amount = Double.Parse(contract.TotalValue.ToString()),
                OrderDescription = $"Thanh toan don hang {CreateTransactionModel.ContractId}",
                Name = contract.CustomerName == null ? "EduToyRent" : contract.CustomerName,
                IpAddress = "127.0.0.1"
            };

            return model;
        }

    }
}
