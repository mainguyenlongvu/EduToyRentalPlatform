using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToyShop.Contract.Repositories.Entity;
using ToyShop.Contract.Services.Interface;
using ToyShop.ModelViews.ToyModelViews;

namespace EduToyRentalPlatform.Pages.Admin.ToyManage
{
    public class IndexModel : PageModel
    {
        private readonly IToyService _toyService;

        public IndexModel(IToyService toyService)
        {
            _toyService = toyService;
        }

        public List<ResponeToyModel> Toys { get; private set; } = new List<ResponeToyModel>();
        public int TotalItems { get; private set; }
        public int PageNumber { get; private set; }
        public int PageSize { get; private set; } = 8; // Default page size

        [BindProperty(SupportsGet = true)]
        public string SearchName { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);

        public async Task OnGetAsync(int pageNumber = 1, int pageSize = 8)
        {
            PageNumber = pageNumber; // Set PageNumber from the parameter
            PageSize = pageSize; // Set PageSize from the parameter

            // Fetch the toys based on current PageNumber and PageSize
            var toys = await _toyService.GetToysAsync(PageNumber, PageSize, true, SearchName);

            // Update TotalItems for the current filter
            TotalItems = toys.TotalItems;

            // If there is a search term, filter the items accordingly
            if (!string.IsNullOrEmpty(SearchName))
            {
                Toys = toys.Items
                    .Where(t => t.ToyName.Contains(SearchName, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                TotalItems = Toys.Count; // Update TotalItems based on filtered results
            }
            else
            {
                Toys = toys.Items.ToList(); // No filter applied
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                await _toyService.DeleteToyAsync(id);
            }
            return RedirectToPage("./Index");
        }
    }
}
