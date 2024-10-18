using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ToyShop.Contract.Repositories.Entity;
using ToyShop.Contract.Services.Interface;
using ToyShop.ModelViews.ToyModelViews;
using ToyShop.Repositories.Base;

namespace EduToyRentalPlatform.Pages.Admin.ToyManage
{
    public class CreateModel : PageModel
    {
        private readonly IToyService _toyService;

        public CreateModel(IToyService toyService)
        {
            _toyService = toyService;
        }

        [BindProperty]
        public CreateToyModel Toy { get; set; }


        public void OnGet()
        {
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


            // Map the form data to a CreateToyModel for service call
            var toyCreateModel = new CreateToyModel
            {
                ToyName = Toy.ToyName,
                ToyDescription = Toy.ToyDescription,
                ToyImg = Toy.ToyImg,
                ToyPrice = Toy.ToyPrice,
                option = Toy.option,
                ToyRemainingQuantity = Toy.ToyRemainingQuantity,
                ToyQuantitySold = Toy.ToyQuantitySold
            };

            // Call the service to create a new toy
            await _toyService.CreateToyAsync(toyCreateModel);

            // Redirect to the ToyManagement page after a successful creation
            return RedirectToPage("/Admin/Product");
        }
    }
}
