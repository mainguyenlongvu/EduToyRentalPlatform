using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToyShop.Contract.Repositories.Entity;
using ToyShop.Contract.Services.Interface;
using ToyShop.ModelViews.ContractModelView; // Assuming you have a view model for contracts

namespace EduToyRentalPlatform.Pages.Admin.ContractManage
{
    public class IndexModel : PageModel
    {
        private readonly IContractService _contractService;

        public IndexModel(IContractService contractService)
        {
            _contractService = contractService;
        }

        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public string SearchName { get; set; }
        public IList<ContractEntity> ContractEntities { get; set; } = new List<ContractEntity>(); // Initialize to avoid null reference

        public async Task OnGetAsync(int pageNumber = 1, int pageSize = 4)
        {
            // Fetch paginated contracts
            var contractsResult = await _contractService.GetContractsAsync(pageNumber, pageSize);

            // Assign results to the properties
            ContractEntities = contractsResult.Items.ToList(); // Assuming your contract service returns a paginated response
            PageNumber = contractsResult.CurrentPage; // Assuming your contract service returns current page
            TotalPages = contractsResult.TotalPages; // Assuming your contract service returns total pages
        }
    }
}
