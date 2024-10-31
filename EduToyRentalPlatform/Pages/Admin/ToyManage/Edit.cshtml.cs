using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToyShop.Contract.Services.Interface;
using ToyShop.ModelViews.ToyModelViews;

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
            // Fetch the toy data based on the provided id
            Toy = await _toyService.GetToyAsync(id);
            if (Toy == null)
            {
                return NotFound(); // Return a 404 if the toy is not found
            }

            return Page(); // Return the page with toy details to edit
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            // Validate the form inputs
            if (!ModelState.IsValid)
            {
                return Page(); // Return the page with validation errors
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

                // Generate a unique file name using a GUID to prevent conflicts
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(Toy.ImageFile.FileName);

                // Full path to save the image
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Save the file
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Toy.ImageFile.CopyToAsync(fileStream);
                }

                // Save the relative path to the ToyImg property
                Toy.ToyImg = uniqueFileName;
            }

            // Prepare the model for the update
            var toyUpdateModel = new UpdateToyModel
            {
                ToyName = Toy.ToyName,
                ToyDescription = Toy.ToyDescription,
                ToyImg = Toy.ToyImg,
                ToyPriceSale = Toy.ToyPriceSale,
                ToyPriceRent = Toy.ToyPriceRent,
                Option = Toy.Option, // Fixed the casing of the property
                ToyRemainingQuantity = Toy.ToyRemainingQuantity,
                ToyQuantitySold = Toy.ToyQuantitySold,
            };

            // Call the service to update the toy
            await _toyService.UpdateToyAsync(Toy.Id, toyUpdateModel);

            // Redirect to the ToyManagement page after a successful update
            return RedirectToPage("/Admin/ToyManage/Index");
        }
    }
}
