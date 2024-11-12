using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToyShop.Contract.Services.Interface;
using ToyShop.ModelViews.ToyModelViews;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToyShop.Contract.Repositories.Entity;
using ToyShop.Contract.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace EduToyRentalPlatform.Pages.Cart
{
    public class CartModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartModel(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
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
    }
}