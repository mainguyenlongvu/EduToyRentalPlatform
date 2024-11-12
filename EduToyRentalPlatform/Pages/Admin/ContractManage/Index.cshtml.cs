using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToyShop.Contract.Repositories.Entity;
using ToyShop.Contract.Services.Interface;
using ToyShop.Core.Base;
using ToyShop.ModelViews.RestoreToyDetailModelViews;
using ToyShop.ModelViews.RestoreToyModelViews;
using ToyShop.ModelViews.TransactionModelView;
using ToyShop.Services.Service;

namespace EduToyRentalPlatform.Pages.Admin.ContractManage
{
    public class IndexModel : PageModel
    {
        private readonly IContractService _contractService;
        private readonly IRestoreToyService _restoreToyService;
        private readonly IRestoreToyDetailService _restoreToyDetailService;
        private readonly ITransactionService _transactionService;

        // Constructor to inject dependencies
        public IndexModel(IContractService contractService,
                          IRestoreToyService restoreToyService,
                          ITransactionService transactionService,
                          IRestoreToyDetailService restoreToyDetailService)
        {
            _contractService = contractService;
            _restoreToyService = restoreToyService;
            _transactionService = transactionService;
            _restoreToyDetailService = restoreToyDetailService;
        }

        public int TotalItems { get; private set; }
        public int PageNumber { get; private set; }
        public int PageSize { get; private set; } = 8; // Default page size

        [BindProperty(SupportsGet = true)]
        public string SearchName { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
        public IList<ContractEntity> ContractEntities { get; set; } = new List<ContractEntity>();

        // Fetch contracts and handle pagination and search
        public async Task OnGetAsync(int pageNumber = 1, int pageSize = 15)
        {
            // Set PageNumber and ensure it is at least 1
            PageNumber = Math.Max(pageNumber, 1);

            // Fetch contracts based on current PageNumber, PageSize, and SearchName
            var contracts = await _contractService.GetContractsAsync(PageNumber, PageSize, SearchName);

            // Update TotalItems based on the fetched contracts
            TotalItems = contracts.TotalItems;

            // If there is a search term, filter the items accordingly
            if (!string.IsNullOrEmpty(SearchName))
            {
                ContractEntities = contracts.Items
                    .Where(c => c.ApplicationUser.FullName.Contains(SearchName, StringComparison.OrdinalIgnoreCase) ||
                                c.Status.Contains(SearchName, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                TotalItems = ContractEntities.Count; // Update TotalItems based on filtered results
            }
            else
            {
                ContractEntities = contracts.Items.ToList(); // No filter applied
            }
        }

        // Method to update restore toy
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

        // Method to update transaction
        public async Task<IActionResult> OnPostUpdateTransaction(int tranCode, string status, string contractId,bool   Method )
        {
            try
            {
                var transactionDTO = new CreateTransactionModel
                {
                    TranCode = tranCode,
                    Status = status,
                    ContractId = contractId,
                    Method = Method

                };

                var updatedTransaction = await _transactionService.Update(tranCode, transactionDTO);

                return RedirectToPage();
            }
            catch (KeyNotFoundException)
            {
                ModelState.AddModelError(string.Empty, "Transaction không tìm thấy.");
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return Page();
        }



        // Method to update restore toy detail
        public async Task<IActionResult> OnPostUpdateRestoreToyDetail(string restoreToyDetailId, UpdateRestoreDetailModel restoreToyDetailDTO)
        {
            try
            {
                await _restoreToyDetailService.Update(restoreToyDetailId, restoreToyDetailDTO);

                return RedirectToPage();
            }
            catch (KeyNotFoundException)
            {
                ModelState.AddModelError(string.Empty, "RestoreToyDetail không tìm thấy.");
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return Page();
        }
    }
}
