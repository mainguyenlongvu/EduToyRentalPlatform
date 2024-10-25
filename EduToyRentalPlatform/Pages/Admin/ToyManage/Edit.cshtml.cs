using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToyShop.Contract.Repositories.Entity;
using ToyShop.Contract.Services.Interface;
using ToyShop.ModelViews.ToyModelViews;
using ToyShop.Repositories.Base;

namespace EduToyRentalPlatform.Pages.Admin.ToyManage
{
    public class EditModel : PageModel
    {
        private readonly IToyService _toyService;

        public EditModel(IToyService toyService)
        {
            _toyService = toyService;
        }

        [BindProperty]
        public ResponeToyModel Toy { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            Toy = await _toyService.GetToyAsync(id);
            if (Toy == null)
            {
                return NotFound();
            }

            return Page();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            // Validate the form inputs
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Handle the image file upload
            if (Toy.ImageFile != null)
            {
                // Define the path where the image will be saved
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/");

                // Ensure the directory exists
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Generate a unique file name
                var uniqueFileName = Toy.ImageFile.FileName;

                // Full path to save the image
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Check if the image already exists
                if (!System.IO.File.Exists(filePath))
                {
                    // Save the file if it doesn't already exist
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await Toy.ImageFile.CopyToAsync(fileStream);
                    }

                    // Save the relative path to the ToyImg property
                    Toy.ToyImg = uniqueFileName;
                }
                else
                {
                    // If the image already exists, use the existing file name
                    Toy.ToyImg = uniqueFileName;
                }
            }

            var toyUpdateModel = new UpdateToyModel
            {
                ToyName = Toy.ToyName,
                ToyDescription = Toy.ToyDescription,
                ToyImg = Toy.ToyImg,
                ToyPriceSale = Toy.ToyPriceSale,
                ToyPriceRent = Toy.ToyPriceRent,
                option = Toy.option,
                ToyRemainingQuantity = Toy.ToyRemainingQuantity,
                ToyQuantitySold = Toy.ToyQuantitySold,
            };

            // Call the service to update the toy
            await _toyService.UpdateToyAsync(Toy.Id, toyUpdateModel);

            // Redirect to the ToyManagement page after a successful update
            return RedirectToPage("/Admin/Product");
        }
    }
}