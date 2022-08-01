namespace ShoppingCart.Models
{
    public class CartItem
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total
        {
            get
            {
                return (decimal)Quantity * Price;
            }
        }

        public string Image { get; set; }

        public CartItem()
        {

        }

        public CartItem(Product product)
        {
            Id          = product.Id;
            Price       = product.Price;
            Name        = product.Name;
            Quantity    = 1;
            Image       = product.Image;

        }
    }
}
