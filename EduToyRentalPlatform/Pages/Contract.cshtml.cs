using ToyShop.Contract.Repositories.Entity;
using ToyShop.Core.Base;
using ToyShop.ModelViews.ContractModelView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToyShop.Contract.Services.Interface;
using ToyShop.Contract.Repositories.PaggingItems;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;
using ToyShop.Services.Service;
using System.Security.Claims;
using ToyShop.ModelViews.RestoreToyModelViews;

namespace ToyShop.Pages
{
    public class ContractModel : PageModel
    {
        private readonly IContractService _contractService;
        private readonly IRestoreToyService _restoreToyService;
        private readonly IRestoreToyDetailService _restoreToyDetailService;
        private readonly ITransactionService _transactionService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        // Constructor to inject dependencies
        public ContractModel(IContractService contractService,
                     IRestoreToyService restoreToyService,
                     ITransactionService transactionService,
                     IRestoreToyDetailService restoreToyDetailService,
                     IHttpContextAccessor httpContextAccessor)
        {
            _contractService = contractService;
            _restoreToyService = restoreToyService;
            _transactionService = transactionService;
            _restoreToyDetailService = restoreToyDetailService;
            _httpContextAccessor = httpContextAccessor;
        }


        public int TotalItems { get; private set; }
        public int PageNumber { get; private set; }
        public int PageSize { get; private set; } = 10;

        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
        public IList<ContractEntity> ContractEntities { get; set; } = new List<ContractEntity>();

        public async Task OnGetAsync(int pageNumber = 1, string searchTerm = null)
        {
            // Get the current user's ID from cookies
            var userId = _httpContextAccessor.HttpContext?.Request.Cookies["UserId"];

            // If user is not logged in (cookie not found), redirect to the login page
            if (string.IsNullOrEmpty(userId))
            {
                RedirectToPage("/Account/LoginPage");  // Redirect to login page if not logged in
                return;
            }

            // Get the contracts for the logged-in user
            var contracts = await _contractService.GetAllContractsByUserIdAsync(userId, searchTerm);

            // Set pagination variables
            TotalItems = contracts.TotalCount;
            PageNumber = pageNumber;

            // Get paginated contracts
            ContractEntities = contracts.Items.ToList();
        }

        public async Task<IActionResult> OnPostCreateReturn(string id, UpdateRestoreModel restoreToy)
        {
            try
            {
                // Assuming the service method handles the logic for updating
                await _restoreToyService.Update(id, restoreToy);
                return RedirectToPage();
            }
            catch (KeyNotFoundException)
            {
                ModelState.AddModelError(string.Empty, "RestoreToy không tìm thấy.");
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return Page();
        }

    }
}
