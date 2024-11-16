using ToyShop.Contract.Repositories.Entity;
using ToyShop.Core.Base;
using ToyShop.ModelViews.ContractModelView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToyShop.Contract.Services.Interface;
using ToyShop.Contract.Repositories.PaggingItems;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;

namespace ToyShop.Pages
{
    public class ContractModel : PageModel
    {
        private readonly ToyShop.Repositories.Base.ToyShopDBContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IContractService _contractService;

        public ContractModel(ToyShop.Repositories.Base.ToyShopDBContext context, IHttpContextAccessor httpContextAccessor, IContractService contractService)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _contractService = contractService;
        }

        public PaginatedList<ContractEntity> Contracts { get; set; } = new PaginatedList<ContractEntity>(new List<ContractEntity>(), 0, 1, 5); // Adjust based on your pagination logic

        public async Task<IActionResult> OnGetAsync(int index = 1, int pageSize = 5, string? searchTerm = null)
        {
            string userId = _httpContextAccessor.HttpContext?.Request.Cookies["UserId"];

            if (string.IsNullOrEmpty(userId))
            {
                // Redirect to login if no user ID is found in cookies
                return RedirectToPage("/Account/Login");
            }

            // Query contracts for the logged-in user
            var query = _context.ContractEntitys
                .Where(c => c.UserId.ToString() == userId) // Filter contracts by user ID
                .OrderBy(c => c.DateCreated);
            var contract = await _contractService.GetAllContractsByUserIdAsync(userId, searchTerm);
            int totalItems = await query.CountAsync();
            Contracts = contract;

            return Page();
        }
    }
}
