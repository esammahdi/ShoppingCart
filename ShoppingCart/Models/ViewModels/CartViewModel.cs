namespace ShoppingCart.Models.ViewModels
{
    public class CartViewModel
    {
        public List<CartItem> cartItems { get; set; }
        public decimal Total { get; set; }
    }
}
