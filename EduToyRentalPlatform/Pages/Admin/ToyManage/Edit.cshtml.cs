using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
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
                return NotFound();
            }
            return Page();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Toy.ImageFile != null)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(Toy.ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Toy.ImageFile.CopyToAsync(fileStream);
                }
                Toy.ToyImg = uniqueFileName;
            }

            var toyUpdateModel = new UpdateToyModel
            {
                ToyName = Toy.ToyName,
                ToyDescription = Toy.ToyDescription,
                ToyImg = Toy.ToyImg,
                ToyPriceSale = Toy.ToyPriceSale,
                ToyPriceRent = Toy.ToyPriceRent,
                Option = Toy.Option,
                ToyRemainingQuantity = Toy.ToyRemainingQuantity,
                ToyQuantitySold = Toy.ToyQuantitySold,
            };

            await _toyService.UpdateToyAsync(Toy.Id, toyUpdateModel);
            return RedirectToPage("/Admin/ToyManage/Index");
        }
    }
}
