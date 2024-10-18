using ToyShop.Contract.Repositories.Entity;
using ToyShop.Core.Base;
using ToyShop.ModelViews.ContractModelView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToyShop.Contract.Services.Interface;

namespace ToyShop.Pages
{
    public class ContractModel : PageModel
    {

        private readonly IContractService _contractService;
        public BasePaginatedList<ToyShop.Contract.Repositories.Entity.ContractEntity>? Contracts { get; set; }
        public ContractModel(IContractService contractService)
        {
            _contractService = contractService;
        }
        public async Task OnGetAsync([FromRoute] int index = 1, [FromRoute] int size = 5)
        {
            Contracts = await _contractService.GetContractsAsync(index, size);
        }

        public async Task<IActionResult> OnPutAsync(string id, [FromForm] UpdateContractModel contract)
        {
            try
            {
                await _contractService.UpdateContractAsync(id, contract);
            }
            catch (BaseException ex)
            {
                return BadRequest(ex.Message);
            }
            return Content("Thành công");
        }

        public async Task<IActionResult> OnDeleteAsync([FromBody] string? id)
        {
            try
            {
                await _contractService.DeleteContractAsync(id);
            }
            catch (BaseException ex)
            {
                return BadRequest("Có lỗi");
            }
            return Content("Xóa thành công");
        }

        [BindProperty]
        private CreateContractModel createNewContract { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _contractService.CreateContractAsync(createNewContract);
            }
            catch (BaseException ex)
            {
                return BadRequest(ex.Message);
            }
            return Content("Thêm thành công");
        }
    }
}
