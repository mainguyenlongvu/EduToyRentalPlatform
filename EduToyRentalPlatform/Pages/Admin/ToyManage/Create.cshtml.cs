using System;
using System.IO;
using System.Linq;
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
        public CreateToyModel Toy { get; set; } = new CreateToyModel(); // Initialize to prevent null reference

        public void OnGet()
        {
            // Any initial logic for GET request can be added here.
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                LogModelErrors(); // Log errors for debugging
                return Page();
            }

            // Check for zero price
            if (Toy.ToyPriceSale <= 0)
            {
                ModelState.AddModelError("Toy.ToyPriceSale", "Sale price must be greater than 0.");
                return Page();
            }

            // Handle the image file upload
            if (Toy.ImageFile != null)
            {
                var validationResult = ValidateImageFile(Toy.ImageFile);
                if (validationResult != null)
                {
                    ModelState.AddModelError("Toy.ImageFile", validationResult);
                    return Page();
                }

                // Define the path where the image will be saved
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/");
                EnsureDirectoryExists(uploadsFolder);

                // Generate a unique file name and save the image
                Toy.ToyImg = await SaveImageFileAsync(Toy.ImageFile, uploadsFolder);
                if (Toy.ToyImg == null)
                {
                    ModelState.AddModelError("Toy.ImageFile", "Error uploading image.");
                    return Page();
                }
            }

            // Map the form data to a CreateToyModel for service call
            var toyCreateModel = new CreateToyModel
            {
                ToyName = Toy.ToyName,
                ToyDescription = Toy.ToyDescription,
                ToyImg = Toy.ToyImg,
                ToyPriceRent = Toy.ToyPriceRent,
                ToyPriceSale = Toy.ToyPriceSale,
                Option = Toy.Option,
                ToyRemainingQuantity = Toy.ToyRemainingQuantity,
                ToyQuantitySold = Toy.ToyQuantitySold
            };

            // Call the service to create a new toy
            await _toyService.CreateToyAsync(toyCreateModel);

            // Redirect to the ToyManagement page after a successful creation
            return RedirectToPage("/Admin/ToyManage/Index");
        }

        private void LogModelErrors()
        {
            // Log model state errors for debugging
            foreach (var entry in ModelState)
            {
                if (entry.Value.Errors.Count > 0)
                {
                    Console.WriteLine($"Error for {entry.Key}: {string.Join(", ", entry.Value.Errors.Select(e => e.ErrorMessage))}");
                }
            }
        }

        private string? ValidateImageFile(IFormFile imageFile)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(fileExtension))
            {
                return "Invalid image type. Only JPG, PNG, and GIF files are allowed.";
            }
            return null; // No validation error
        }

        private void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private async Task<string?> SaveImageFileAsync(IFormFile imageFile, string uploadsFolder)
        {
            var uniqueFileName = $"{Guid.NewGuid()}_{imageFile.FileName}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }
                return uniqueFileName; // Return the unique file name for storage
            }
            catch (Exception ex)
            {
                // Handle exception (you could log it)
                Console.WriteLine($"Error saving file: {ex.Message}");
                return null; // Indicate failure
            }
        }
    }
}
