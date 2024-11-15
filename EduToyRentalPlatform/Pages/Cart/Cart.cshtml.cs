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

        public IActionResult OnPostCheckout()
        {
            TempData["CartItems"] = MyCart;
            return RedirectToPage("/Cart/Checkout");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Iterate through posted values to update each item's ContractType
            foreach (var item in MyCart)
            {
                string radioValue = Request.Form[$"purchaseOption_{item.Toy.Id}"];

                if (bool.TryParse(radioValue, out bool selectedType))
                {
                    item.ContractType = selectedType;
                }
            }

            // Save changes to database or session as needed
            await SaveCartChangesAsync();

            return Page();
        }

        public async Task SaveCartChangesAsync()
        {
            // Assuming the MyCart list contains ContractDetails that need to be updated
            foreach (var item in MyCart)
            {
                // Update the ContractDetail with the new values (e.g., price and quantity)
                _unitOfWork.GetRepository<ContractDetail>().Update(item);
            }

            // Save changes to the database
            await _unitOfWork.SaveAsync();
        }

    }
    }