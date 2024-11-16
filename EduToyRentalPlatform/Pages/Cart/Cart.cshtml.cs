using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToyShop.Contract.Services.Interface;
using ToyShop.ModelViews.ToyModelViews;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToyShop.Contract.Repositories.Entity;
using ToyShop.Contract.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components;

namespace EduToyRentalPlatform.Pages.Cart
{
    public class CartModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IContractService _contractService;
        private readonly IContractDetailService _contractDetailService;

        public CartModel(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IContractService contractService, IContractDetailService contractDetailService)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _contractService = contractService;
            _contractDetailService = contractDetailService;
        }
        [BindProperty]
        public string ItemId { get; set; }
        public List<ContractDetail> MyCart { get; set; }
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
                    MyCart = contract.Result.ContractDetails.Where(x=>!x.DeletedTime.HasValue).ToList();
                }
            }
            else
            {
                 Response.Redirect("/Account/LoginPage");
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(/*string itemId*/)
        {
            if (string.IsNullOrEmpty(ItemId))
            {
                return BadRequest("Item ID is missing."); // HTTP 400
            }

            try
            {
                await _contractDetailService.DeleteContractDetailAsync(ItemId);
                return RedirectToPage("/Cart/Cart");
            }
            catch (Exception ex)
            {
                // Ghi log lỗi để kiểm tra (tùy chọn)
                return RedirectToPage("/Cart/Cart");
            }
        }



    }
}