using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToyShop.Contract.Services.Interface;
using ToyShop.ModelViews.ToyModelViews;

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
            try
            {
                // Handle the image upload
                await HandleImageUploadAsync();

                // Call the service to create a new toy
                await _toyService.CreateToyAsync(Toy);

                // Redirect to the ToyManagement page after a successful creation
                return RedirectToPage("/Admin/ToyManage/Index");
            }
            catch (Exception ex)
            {
                // Log the error (consider using a logging framework)
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }

        private async Task HandleImageUploadAsync()
        {
            // Check if an image file is uploaded
            if (Toy.ImageFile != null)
            {
                // Define the path where the image will be saved
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/");

                // Ensure the directory exists
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Generate a unique file name using GUID
                var uniqueFileName = Guid.NewGuid() + Path.GetExtension(Toy.ImageFile.FileName);

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
        }
    }
}
