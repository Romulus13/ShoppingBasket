
using ShoppingBasket.ProductModel;

namespace ShoppingBasket
{
    public interface IShoppingCart
    {

        void Add(Product product);
        void Remove(Product product);
        void CalculatePrices();
        bool AddDiscount(ShoppingBasket.DiscountModel.Discount discount);

        bool RemoveDiscount(ShoppingBasket.DiscountModel.Discount discount);

        void ApplyDiscounts();

        void CancelDiscounts();
    
    }
}