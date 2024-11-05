using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToyShop.Contract.Services.Interface;
using ToyShop.ModelViews.ToyModelViews;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToyShop.Contract.Repositories.Entity;

namespace EduToyRentalPlatform.Pages.Cart
{
    public class CartModel : PageModel
    {
        private readonly IToyService _toyService;

        public CartModel(IToyService toyService)
        {
            _toyService = toyService;
        }
        public List<Item> MyCart { get; set; } = new();
        public void OnGet()
        {
            MyCart = SessionExtensions.GetObject<List<Item>>(HttpContext.Session, "cart");
        }
        public async Task<IActionResult> OnGetBuy(string id)
        {
            var item = await _toyService.GetToyAsync(id);
            var cart = SessionExtensions.GetObject<List<Item>>(HttpContext.Session, "cart");
            if (cart == null)
            {
                cart = new List<Item>();
                cart.Add(new Item()
                {
                    Toy = item,
                    Quantity = 1
                });
                SessionExtensions.SetObject(HttpContext.Session, "cart", cart);
            }
            else
            {
                var index = Exists(cart, id);
                if (index == -1)
                {
                    cart.Add(new Item()
                    {
                        Toy = item,
                        Quantity = 1
                    });
                }
                else
                {
                    var newQuantity = cart[index].Quantity + 1;
                    cart[index].Quantity = newQuantity;
                    SessionExtensions.SetObject(HttpContext.Session, "cart", cart);
                }
                SessionExtensions.SetObject(HttpContext.Session, "cart", cart);
            }
                return RedirectToPage("Cart");
            }

            private int Exists(List<Item> cart, string id)
            {
                for (int i = 0; i < cart.Count; i++)
                {
                    if (cart[i].Toy.Id.Equals(id))
                    {
                        return i;
                    }
                }
                return -1;
            }
        }
    }