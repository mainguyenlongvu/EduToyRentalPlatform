using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToyShop.Contract.Services.Interface;
using ToyShop.ModelViews.ToyModelViews;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToyShop.Contract.Repositories.Entity;
using ToyShop.Contract.Repositories.Interface;

namespace EduToyRentalPlatform.Pages.Cart
{
    public class CartModel : PageModel
    {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IHttpContextAccessor _httpContextAccessor;

        public CartModel(IToyService toyService)
        {
            _toyService = toyService;
        }
        public List<Item>? MyCart { get; set; } = new();
        public void OnGet()
        {
            MyCart = SessionExtensions.GetObject<List<Item>>(HttpContext.Session, "cart");
        }
        public async Task<IActionResult> OnGetBuy(string id)
        {
            var item = await _toyService.GetToyAsync(id);
            var cart = SessionExtensions.GetObject<List<Item>>(HttpContext.Session, "cart");
            if (cart == null)
            {
                _unitOfWork = unitOfWork;
                _httpContextAccessor = httpContextAccessor;
            }

            public List<ContractDetail> MyCart { get; set; }

            public async Task OnGetAsync()
            {
                // Retrieve the current user ID from cookies
                string userId = _httpContextAccessor.HttpContext?.Request.Cookies["UserId"];

                if (!string.IsNullOrEmpty(userId))
                {
                    // Find the contract with status "In Cart" for this user
                    var contract = await _unitOfWork.GetRepository<ContractEntity>()
                        .Entities
                        .Include(c => c.ContractDetails)
                            .ThenInclude(d => d.Toy)
                        .FirstOrDefaultAsync(x => x.UserId.ToString() == userId && x.Status == "In Cart");

                    if (contract != null)
                    {
                        // Populate MyCart with contract details
                        MyCart = contract.ContractDetails.ToList();
                    }
                }
            }

        public IActionResult OnPostCheckout()
        {
            TempData["CartItems"] = MyCart;
            return RedirectToPage("/Cart/Checkout");
        }
    }
    }