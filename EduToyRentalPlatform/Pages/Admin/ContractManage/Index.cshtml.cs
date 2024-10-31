using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToyShop.Contract.Repositories.Entity;
using ToyShop.Contract.Services.Interface;
using ToyShop.Core.Base;
using ToyShop.ModelViews.ContractDetailModelView;

namespace EduToyRentalPlatform.Pages.Admin.ContractManage
{
    public class IndexModel : PageModel
    {
        private readonly IContractService _contractService;

        public IndexModel(IContractService contractService)
        {
            _contractService = contractService;
        }

        public IList<ContractEntity> ContractEntities { get; set; } = default!;

        public async Task OnGetAsync()
        {
            ContractEntities = await _contractService.GetAllContractsAsync();
        }

      
    }
}
