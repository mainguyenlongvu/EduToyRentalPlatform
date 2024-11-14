using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ToyShop.Contract.Services.Interface;
using ToyShop.Repositories.Base;
using ToyShop.Repositories.Entity;

namespace EduToyRentalPlatform.Pages.Admin.UserManage
{
    public class DeleteModel : PageModel
    {
        private readonly IUserService _userService;

        public DeleteModel(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            // Attempt to delete feedback
            bool isDeleted = await _userService.DeleteUserAsync(id);

            // Redirect to the Index page if deletion was successful
            if (isDeleted)
            {
                return RedirectToPage("Index");
            }

            // If deletion failed, return to the same page (could also show an error message)
            return Page();
        }
    }
}
