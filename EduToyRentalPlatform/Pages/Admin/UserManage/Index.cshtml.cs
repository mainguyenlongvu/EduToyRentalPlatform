using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToyShop.Contract.Services.Interface;
using ToyShop.Repositories.Entity;

namespace EduToyRentalPlatform.Pages.Admin.UserManage
{
    public class IndexModel : PageModel
    {
        private readonly IUserService _userService;

        public IndexModel(IUserService userService)
        {
            _userService = userService;
        }

        public IList<ApplicationUser> ApplicationUsers { get; set; } = new List<ApplicationUser>();

        // For pagination
        public int TotalItems { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 10; // Default page size

        [BindProperty(SupportsGet = true)]
        public string SearchName { get; set; } // For search functionality

        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);

        public async Task OnGetAsync(int pageNumber = 1)
        {
            PageNumber = pageNumber;

            // Fetching the users based on pagination and search
            var result = await _userService.GetPageAsync(PageNumber, PageSize, SearchName);

            ApplicationUsers = result.Items.ToList();
            TotalItems = result.TotalItems;
        }

        // If you need to implement delete functionality for users
        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                await _userService.DeleteUserAsync(id);
            }
            return RedirectToPage("./Index");
        }
    }
}
