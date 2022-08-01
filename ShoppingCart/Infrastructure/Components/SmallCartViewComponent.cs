using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models;
using ShoppingCart.Models.ViewModels;

namespace ShoppingCart.Infrastructure.Components
{
    public class SmallCartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");
            
            SmallCartViewModel? smallCartViewModel;

            if (cart == null || cart.Count == 0)
            {
                smallCartViewModel = null;
            } else
            {
                smallCartViewModel = new()
                {
                    NumberOfItems = cart.Sum(cartItem => cartItem.Quantity),
                    TotalAmount = cart.Sum(cartItem => cartItem.Quantity * cartItem.Price)
                };

            }

            return View(smallCartViewModel);

        }
    }
}
