using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
        private IContractDetailService _contractDetailService;
        private ITransactionService _transactionService;


        #region constructors
        public CheckoutModel(IVnPayService vnPayService)
        {
            _vnPayService = vnPayService;
        }

        public CheckoutModel(IVnPayService vnPayService, IContractService contractService, IContractDetailService contractDetailService, ITransactionService transactionService) : this(vnPayService)
        {
            _contractService = contractService;
            _contractDetailService = contractDetailService;
            _transactionService = transactionService;
        }

        #endregion

        [BindProperty]
        public bool IsTopUp { get; set; }

        [BindProperty]
        public CreateTopUpModel CreateTopUpModel { get; set; }

        [BindProperty]
        public CreateContractModel CreateContractModel { get; set; }

        [BindProperty]
        public CreateContractDetailModel CreateContractDetailModel { get; set; }

        [BindProperty]
        public CreateTransactionModel CreateTransactionModel { get; set; }


        public void OnGet()
        {
        
        }

        [ValidateAntiForgeryToken]
        public async Task OnPost() 
        {
            
            Console.WriteLine("Payment OnPost Called");
            var model =  IsTopUp? await CreateVnPayTopUpRequest() : await CreateVnPayPurchaseRequest();
            string url = CreatePaymentUrl(model, HttpContext);
            
            Response.Redirect(url);         
        }



        private string CreatePaymentUrl(VnPayRequestModel model, HttpContext context)
        {
            string url = _vnPayService.CreatePaymentUrl(model, context);
            return url;
        }

        private async Task<VnPayRequestModel> CreateVnPayTopUpRequest()
        {
            var contractModel = new CreateContractModel()
            {
                CustomerName = CreateTopUpModel.CustomerName,
                TotalValue = CreateTopUpModel.TotalValue,
            };

            await _contractService.CreateContractAsync(CreateContractModel);
            await _transactionService.Insert(CreateTransactionModel);

            var model = new VnPayRequestModel()
            {
                OrderType = "260000", // https://sandbox.vnpayment.vn/apis/docs/loai-hang-hoa/
                Amount = Double.Parse(CreateTopUpModel.TotalValue.ToString()),
                OrderDescription = $"Thanh toan nap vi {CreateTransactionModel.TranCode}",
                Name = CreateTopUpModel.CustomerName == null ? "EduToyRent" : CreateTopUpModel.CustomerName,
                IpAddress = "127.0.0.1"
            };

            return model;
        }

        private async Task<VnPayRequestModel> CreateVnPayPurchaseRequest()
        {
            await _contractService.CreateContractAsync(CreateContractModel);
            await _contractDetailService.CreateContractDetailAsync(CreateContractDetailModel);
            await _transactionService.Insert(CreateTransactionModel);

            var model = new VnPayRequestModel()
            {
                OrderType = "190000", // https://sandbox.vnpayment.vn/apis/docs/loai-hang-hoa/
                Amount = Double.Parse(CreateContractModel.TotalValue.ToString()),
                OrderDescription = $"Thanh toan don hang {CreateTransactionModel.TranCode}",
                Name = CreateContractModel.CustomerName == null ? "EduToyRent" : CreateContractModel.CustomerName,
                IpAddress = "127.0.0.1"
            };

            return model;
        }

    }
}
