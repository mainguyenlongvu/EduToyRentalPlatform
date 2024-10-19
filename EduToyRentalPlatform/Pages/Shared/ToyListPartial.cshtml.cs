using Microsoft.AspNetCore.Mvc.RazorPages;
using ToyShop.Contract.Services.Interface;
using ToyShop.ModelViews.ToyModelViews;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToyShop.Pages
{
    public class ToyListModel : PageModel
    {
        private readonly IToyService _toyService;

        public List<ResponeToyModel> Toys { get; set; }

        public ToyListModel(IToyService toyService)
        {
            _toyService = toyService;
        }

        public async Task OnGetAsync(int pageNumber = 1, int pageSize = 10, bool? sortByName = null)
        {
            // Use the toy service to get the list of toys
            var paginatedToys = await _toyService.GetToysAsync(pageNumber, pageSize, sortByName);

            // Assign the retrieved list of toys to the Toys property
            Toys = paginatedToys.Items.ToList();
        }
    }
}
