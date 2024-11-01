using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToyShop.Contract.Repositories.Entity;
using ToyShop.Contract.Services.Interface;
using ToyShop.Core.Base;

namespace EduToyRentalPlatform.Pages.Admin.ContractManage
{
    public class IndexModel : PageModel
    {
        private readonly IContractService _contractService;

        public IndexModel(IContractService contractService)
        {
            _contractService = contractService;
        }

        public int TotalItems { get; private set; }
        public int PageNumber { get; private set; }
        public int PageSize { get; private set; } = 8; // Default page size

        [BindProperty(SupportsGet = true)]
        public string SearchName { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
        public IList<ContractEntity> ContractEntities { get; set; } = new List<ContractEntity>();

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
    }
}
