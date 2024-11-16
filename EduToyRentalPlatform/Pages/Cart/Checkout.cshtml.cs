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
        public ContractEntity contractEntity { get; set; }
        public async Task OnGetAsync()
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
                contractEntity = contract.Result;
            }
        }

        public async Task<IActionResult> OnPostAsync(string contractId, int totalValue, string paymentMethod)
        {
            Console.WriteLine("Payment OnPost Called");
            _httpContextAccessor.HttpContext?.Session.SetString("OrderType", "Purchase");

            // Kiểm tra dữ liệu đầu vào
            if (!ModelState.IsValid)
            {
                return RedirectToPage("/Cart/Checkout");
            }

            // Lấy thông tin UserId từ Cookie
            var userId = _httpContextAccessor.HttpContext?.Request.Cookies["UserId"];
            if (string.IsNullOrEmpty(userId))
            {
                throw new KeyNotFoundException("UserId không tồn tại.");
            }

            HttpContext.Session.SetString("UserId", userId);

            // Tạo Transaction Model
            var tranModel = new CreateTransactionModel()
            {
                TranCode = int.Parse(new Random().NextInt64(100000000, 999999999).ToString()),
                ContractId = contractId,
            };

            // Xử lý từng phương thức thanh toán
            if (paymentMethod.Equals("VNPay", StringComparison.OrdinalIgnoreCase)) // Thanh toán VNPay
            {
                HttpContext.Session.SetObject("CreateTransactionModel", tranModel);

                var contract = await _contractService.GetContractAsync(contractId);
                if (contract == null)
                {
                    ModelState.AddModelError(string.Empty, "Không tìm thấy hợp đồng.");
                    return RedirectToPage("/Cart/Checkout");
                }

                var model = await CreateVnPayPurchaseRequest(tranModel, contract);

                string url = CreatePaymentUrl(model, HttpContext);
                return Redirect(url);
            }

            if (paymentMethod.Equals("Wallet", StringComparison.OrdinalIgnoreCase)) // Thanh toán ví
            {
                bool result = await _transactionService.ProcessPurchaseWallet(tranModel, userId);
                if (!result)
                {
                    ModelState.AddModelError(string.Empty, "Thanh toán qua ví thất bại.");
                    return RedirectToPage("/Cart/Checkout");
                }

                return RedirectToPage("/Cart/Checkout");
            }

            if (paymentMethod.Equals("Direct", StringComparison.OrdinalIgnoreCase)) // Thanh toán trực tiếp
            {
                 await _contractService.DirectPaymentAsync(tranModel.ContractId);

                return RedirectToPage("/Contract");
            }

            // Trường hợp không khớp phương thức thanh toán nào
            ModelState.AddModelError(string.Empty, "Phương thức thanh toán không hợp lệ.");
            return RedirectToPage("/Cart/Checkout");
        }

        


        #region vnpay
        private string CreatePaymentUrl(VnPayRequestModel model, HttpContext context)
        {
            string url = _vnPayService.CreatePaymentUrl(model, context);
            return url;
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
