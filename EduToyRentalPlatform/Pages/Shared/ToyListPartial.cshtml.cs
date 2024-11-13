using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToyShop.Contract.Services.Interface;
using ToyShop.ModelViews.ContractDetailModelView;

namespace ToyShop.Pages.Shared
{
    public class ToyListPartialModel : PageModel
    {
        private readonly IContractService _contractService;
        private readonly IContractDetailService _contractDetailService;

        // Sử dụng [BindProperty] để nhận giá trị từ form
        [BindProperty]
        public string ToyId { get; set; }

        [BindProperty]
        public int Quantity { get; set; }

        public ToyListPartialModel(IContractService contractService, IContractDetailService contractDetailService)
        {
            _contractService = contractService;
            _contractDetailService = contractDetailService;
        }

        public void OnGet()
        {
        }

        // Phương thức xử lý POST cho Add to Cart
        public async Task<IActionResult> OnPostAsync()
        {
            // Kiểm tra nếu ToyId và Quantity có dữ liệu
            if (!string.IsNullOrEmpty(ToyId) && Quantity > 0)
            {
                var model = new CreateContractDetailModel
                {
                    ToyId = ToyId,
                    Quantity = Quantity,
                    ContractType = true
                };

                await _contractDetailService.CreateContractDetailAsync(model);

                // Chuyển hướng đến trang Giỏ Hàng
                return RedirectToPage("/Cart/Cart");
            }

            // Nếu dữ liệu không hợp lệ, trả về lại trang hiện tại
            return Page();
        }
    }
}
