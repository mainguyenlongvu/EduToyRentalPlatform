using System.Collections.Generic; // Required for List<>
using System.Linq; // Required for LINQ
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

        public List<ContractEntity> ContractEntities { get; set; } = new List<ContractEntity>(); // Initialize to avoid null reference
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize); // Calculate total pages

        public async Task OnGetAsync(int pageNumber = 1, int pageSize = 10) // Only keep this method
        {
            // Validate pageNumber and pageSize to ensure they are within expected limits
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            if (pageSize < 1 || pageSize > 100) // Assuming 100 is the maximum page size
            {
                pageSize = 10; // Reset to default if invalid
            }

            // Fetch the paginated contracts using the contract service
            var paginatedContracts = await _contractService.GetContractsAsync(pageNumber, pageSize);
            ContractEntities = paginatedContracts.Items.ToList(); // Assuming your service returns a paginated list with an Items property
            TotalItems = paginatedContracts.TotalItems; // Assuming your service provides total items
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
