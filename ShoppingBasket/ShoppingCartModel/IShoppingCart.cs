
using ShoppingBasket.DiscountModel;
using ShoppingBasket.ProductModel;

namespace ShoppingBasket.ShopppingCartModel
{
    public interface IShoppingCart<T> where T: class
    {

        void Add(Product product);
        void Remove(Product product);
        void CalculatePrices();
        bool AddDiscount(Discount<T> discount);

        bool RemoveDiscount(Discount<T> discount);

        void ApplyDiscounts();

        void CancelDiscounts();
    
    }
}