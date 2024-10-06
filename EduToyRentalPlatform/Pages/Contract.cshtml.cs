
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToyShop.Contract.Repositories.Entity;
using ToyShop.Contract.Services.Interface;
using ToyShop.Core.Base;
using ToyShop.ModelViews.ContractModelView;

namespace ToyShop.Pages
{
    public class ContractModel : PageModel
    {
        private readonly IContractService _contractService;
        public ContractModel(IContractService contractService)
        {
            _contractService = contractService;
        }
        public BasePaginatedList<ContractEntity>? Contracts { get; set; }
       
        public async Task OnGet([FromRoute] int index = 1, [FromRoute] int size = 5)
        {
            Contracts = await _contractService.GetContractsAsync(index, size);
        }

        public  async Task<IActionResult> OnPut(string id, [FromForm] UpdateContractModel contract)
        {
            try { 
                await _contractService.UpdateContractAsync(id, contract);
            }
            catch(BaseException ex)
            {
                return BadRequest(ex.Message);
            }
           return Content("Thanh cong");
        }

        public async Task<IActionResult> OnDelete([FromRoute] string? id)
        {
            try { 
                await _contractService.DeleteContractAsync(id);
            }
            catch(BaseException ex)
            {
                return BadRequest(ex.Message);
            }
            return Content("Xóa thành công");
        }

        public async Task<IActionResult> OnPost(CreateContractModel contract)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _contractService.CreateContractAsync(contract);

            }
            catch(BaseException ex)
            {
                return BadRequest(ex.Message);
            }
            return Content("Thêm thành công");
        }
    }
}
