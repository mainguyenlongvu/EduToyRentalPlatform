using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToyShop.Contract.Services.Interface;
using ToyShop.ModelViews.ContractDetailModelView;
using ToyShop.ModelViews.FeedBackModelViews;
using ToyShop.ModelViews.ToyModelViews;

namespace EduToyRentalPlatform.Pages
{
    public class ShopDetailModel : PageModel
    {
        private readonly IToyService _toyService;
        private readonly IFeedBackService _feedBackService;
        private readonly IContractDetailService _contractDetailService;

        public ResponeToyModel Toy { get; set; }
        [BindProperty]
        public CreateFeedBackModel Feedback { get; set; }
        public List<ResponeFeedBackModel> Feedbacks { get; set; } = new List<ResponeFeedBackModel>();
        [BindProperty]
        public string ToyId { get; set; } // ToyId sẽ được lấy từ form
        [BindProperty]
        public string PurchaseType { get; set; } // "buy" or "rent"

        [BindProperty]
        public int? BuyQuantity { get; set; } // Quantity if Buy is selected

        [BindProperty]
        public int? RentQuantity { get; set; } // Quantity if Rent is selected

        [BindProperty]
        public DateTime? StartDate { get; set; } // Rent start date

        [BindProperty]
        public DateTime? EndDate { get; set; } // Rent end date
        // Phân trang
        public int TotalItems { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }

        public ShopDetailModel(IToyService toyService, IFeedBackService feedBackService, IContractDetailService contractDetailService)
        {
            _toyService = toyService;
            _feedBackService = feedBackService;
            _contractDetailService = contractDetailService;
        }

        public async Task<IActionResult> OnGetAsync(string id, int pageNumber = 1, int pageSize = 4)
        {
            Toy = await _toyService.GetToyAsync(id);
            if (Toy == null)
            {
                return NotFound();
            }

            var feedbackList = await _feedBackService.GetFeedBacksByToyIdAsync(id, pageNumber, pageSize, null);
            Feedbacks = feedbackList.Items.ToList();
            TotalItems = feedbackList.TotalItems;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = feedbackList.TotalPages;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || Feedback == null)
            {
                return Page();
            }

            var UserId = Request.Cookies.ContainsKey("UserId")
               ? Request.Cookies["UserId"]
               : null;

            if (string.IsNullOrEmpty(UserId))
            {
                TempData["ErrorMessage"] = "You'll need to sign in to send feedback.";
                return RedirectToPage("/Account/LoginPage"); // Redirect to the login page
            }

            var feedback = new CreateFeedBackModel
            {
                UserId = UserId,
                ToyId = Feedback.ToyId,
                Content = Feedback.Content
            };

            await _feedBackService.CreateFeedBackAsync(feedback);
            return RedirectToPage("ShopDetail", new { id = Feedback.ToyId });
        }
        public async Task<IActionResult> OnPostAddToCartAsync()
        {
            if (PurchaseType == "buy")
            {
                // Xử lý logic cho mua
                int quantity = BuyQuantity ?? 1; // Mặc định là 1 nếu không được nhập
                                                 // Thực hiện hành động mua
                CreateContractDetailModel createContractDetailModel = new CreateContractDetailModel
                {
                    ContractType = true,
                    ToyId = ToyId,
                    Quantity = quantity,
                };
                await _contractDetailService.CreateContractDetailAsync(createContractDetailModel);
            }
            else if (PurchaseType == "rent")
            {
                // Xử lý logic cho thuê
                int quantity = RentQuantity ?? 1; // Mặc định là 1 nếu không được nhập
                DateTime? startDate = StartDate;
                DateTime? endDate = EndDate;
                CreateContractDetailModel createContractDetailModel = new CreateContractDetailModel
                {
                    ContractType = false,
                    ToyId = ToyId,
                    Quantity = quantity,
                    DateStart = startDate,
                    DateEnd = endDate,
                };
                // Thực hiện hành động thuê
                await _contractDetailService.CreateContractDetailAsync(createContractDetailModel);
            }

            // Điều hướng hoặc cập nhật trang tùy ý
            return RedirectToPage("/Cart/Cart");
        }
    }
}
