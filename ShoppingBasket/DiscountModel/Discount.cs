using System;
using System.Collections.Generic;
using System.Text;
using ShoppingBasket.ProductModel;
using System.Linq;
using ShoppingBasket.ShopppingCartModel;

namespace ShoppingBasket.DiscountModel
{
    public abstract class Discount<T> where T : class
    {

        private string _name;

        private List<T> _conditionDiscounts;

        /// <summary>
        /// Discount percent - 0 means no discount and 100 means the product is free
        /// </summary>
        private decimal _discountPercent;
        public decimal DiscountPercent { get => _discountPercent; set => _discountPercent = value; }
        public string Name { get => _name; set => _name = value; }
        public List<T> ConditionDiscounts { get => _conditionDiscounts; set => _conditionDiscounts = value; }

        /// <summary>
        /// This is the main callable method on discount. It checks the condition for the discount. Then tries to apply it on the products affected by it.
        /// At the end we remove both condition products and the discounted products from the candidates of products sought after for the discount.
        /// </summary>
        /// <param name="cart"></param>
        /// <returns></returns>
        public abstract bool ApplyDiscountOnCart(ShoppingCart<T> cart);
   

        /// <summary>
        /// Check if the condition for a discount exists
        /// </summary>
        /// <param name="cart"></param>
        protected abstract T FindDiscountCondition(ShoppingCart<T> cart);
        /// <summary>
        /// Apply discount. Remove the 
        /// </summary>
        /// <param name="cart"></param>
        protected abstract void DiscountProduct(ShoppingCart<T> cart, T condition);

        public abstract void RemoveAllConditionDiscounts();

        public abstract bool IsDiscountApplied();

    }
}
