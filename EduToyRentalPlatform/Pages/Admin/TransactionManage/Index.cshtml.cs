using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToyShop.ModelViews.TransactionModelView;
using ToyShop.Services.Service;
using ToyShop.Core.Base;
using ToyShop.Contract.Services.Interface;
using Microsoft.AspNetCore.Mvc; // Ensure this is included if you're using BasePaginatedList

namespace EduToyRentalPlatform.Pages.Admin.TransactionManage
{
    public class IndexModel : PageModel
    {
            private readonly ITransactionService _transactionService;

            public IndexModel(ITransactionService transactionService)
            {
            _transactionService = transactionService;
            }

            public List<ResponseTransactionModel> Transactions { get; set; } = new List<ResponseTransactionModel>();
            public int PageNumber { get; set; }
            public int TotalPages { get; set; }

            //[BindProperty(SupportsGet = true)]
            public string SearchName { get; set; }
            public async Task OnGetAsync(int pageNumber = 1, int pageSize = 4)
            {
                var transactions = await _transactionService.GetPaging(pageNumber, pageSize);

                    Transactions = transactions.Items.ToList();
                    PageNumber = transactions.CurrentPage;
                    TotalPages = transactions.TotalPages;
                }
            

    public async Task<IActionResult> OnGetDeleteAsync(string id)
    {
        if (!string.IsNullOrEmpty(id))
        {
            await _transactionService.Delete(id); // Assuming your Delete method takes a string ID
        }
        return RedirectToPage("/Admin/TransactionManage/Index"); // Redirect to the correct page after deletion
    }
}

    }
