using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics.Contracts;
using ToyShop.Contract.Repositories.Entity;
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
        private IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        #region constructors

        public CheckoutModel(IVnPayService vnPayService, IContractService contractService, ITransactionService transactionService, IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _vnPayService = vnPayService;
            _contractService = contractService;
            _transactionService = transactionService;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        public List<ContractDetail> CartItems { get; set; }

        public void OnGet()
        {
            string userId = _httpContextAccessor.HttpContext?.Request.Cookies["UserId"];

            if (!string.IsNullOrEmpty(userId))
            {
                // Find the contract with status "In Cart" for this user
                var contract = _contractService.GetContractDetailInCart();

                if (contract != null)
                {
                    // Populate MyCart with contract details
                    CartItems = contract.Result.ContractDetails.Where(x => !x.DeletedTime.HasValue).ToList();
                }
            }
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task OnPost(string contractId, string paymentMethod)
        {
            Console.WriteLine("Payment OnPost Called");

            if (!ModelState.IsValid)
            {
                return;
            }

            string userId = _httpContextAccessor.HttpContext?.Request.Cookies["UserId"];
            if (userId == null)
                throw new KeyNotFoundException("UserId not found");
            else
                HttpContext.Session.SetString("UserId", userId);

            var tranModel = new CreateTransactionModel()
            {
                TranCode = int.Parse(new Random().NextInt64(100000000, 999999999).ToString()),
                ContractId = contractId,
            };


            if (paymentMethod.Equals("VNPay")) //vnpay
            {
                HttpContext.Session.SetObject("CreateTransactionModel", tranModel);

                var contract = await _contractService.GetContractAsync(contractId);
                var model = contract.ToyName == null ?
                    await CreateVnPayTopUpRequest(tranModel, contract)
                    :
                    await CreateVnPayPurchaseRequest(tranModel, contract);

                string url = CreatePaymentUrl(model, HttpContext);
                Response.Redirect(url);            
            }

            if (paymentMethod.Equals("Wallet")) //ví
            {
                bool result = await _transactionService.ProcessPurchaseWallet(tranModel, userId);
                if (!result)
                {
                    Response.Redirect("Cart/TestFailed");
                }
                Response.Redirect("Cart/TestSuccess");
            }

            if (paymentMethod.Equals("Direct")) //thanh toán bằng tiền mặt
            {
                bool result = await _transactionService.ProcessPurchaseDirect(tranModel);
                if (!result)
                {
                    Response.Redirect("Cart/TestFailed");
                }
                else Response.Redirect("Cart/TestSuccess");
            } 
        }
        #region vnpay
        private string CreatePaymentUrl(VnPayRequestModel model, HttpContext context)
        {
            string url = _vnPayService.CreatePaymentUrl(model, context);
            return url;
        }

        private async Task<VnPayRequestModel> CreateVnPayTopUpRequest(CreateTransactionModel tranModel, ResponseContractModel contract)
        {
            var model = new VnPayRequestModel()
            {
                OrderType = "260000", // https://sandbox.vnpayment.vn/apis/docs/loai-hang-hoa/
                Amount = Double.Parse(contract.TotalValue.ToString()),
                OrderDescription = $"Thanh toan nap vi {tranModel.ContractId}",
                Name = tranModel.TranCode.ToString(),
                IpAddress = "127.0.0.1"
            };

            return model;
        }

        private async Task<VnPayRequestModel> CreateVnPayPurchaseRequest(CreateTransactionModel tranModel, ResponseContractModel contract)
        {
            var model = new VnPayRequestModel()
            {
                OrderType = "190000", // https://sandbox.vnpayment.vn/apis/docs/loai-hang-hoa/
                Amount = Double.Parse(contract.TotalValue.ToString()),
                OrderDescription = $"Thanh toan don hang {tranModel.ContractId}",
                Name = tranModel.TranCode.ToString(),
                IpAddress = "127.0.0.1"
            };
            return model;
        }
        #endregion

        }
    }
