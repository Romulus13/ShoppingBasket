using System;
using System.Collections.Generic;
using System.Text;
using ShoppingBasket.ProductModel;
using System.Linq;
namespace ShoppingBasket.DiscountModel
{
    public abstract class Discount
    {
        /// <summary>
        /// Discount percent - 0 means no discount and 100 means the product is free
        /// </summary>
        private decimal _discountPercent;

        private List<Product> _discountedProducts;
        private List<Product> _discountConditionProducts;

        public List<Product> DiscountedProducts { get => _discountedProducts; set => _discountedProducts = value; }

        public List<Product> DiscountConditionProducts { get => _discountConditionProducts; set => _discountConditionProducts = value; }
        public bool DiscountApplied
        {
            get
            {
                if (this.DiscountedProducts != null)
                    return this.DiscountedProducts.Count > 0;
                else
                    return false;
            }
        }


        /// <summary>
        ///  This variable states if the condition has been satisfied   
        /// </summary>
        private bool _conditionSatisifed;

      


        public bool ConditionSatisfied
        {
            get
            {
                return _conditionSatisifed;
            }
            set
            {
                this._conditionSatisifed = value;
            }
        }

        public decimal DiscountPercent { get => _discountPercent; set => _discountPercent = value; }

        /// <summary>
        /// This is the main callable method on discount. It checks the condition for the discount. Then tries to apply it on the products affected by it.
        /// At the end we remove both condition products and the discounted products from the candidates of products sought after for the discount.
        /// </summary>
        /// <param name="cart"></param>
        /// <returns></returns>
        public virtual bool ApplyDiscountOnCart(ShoppingCart cart)
        {
            bool lackingProducts = cart == null || cart.Products == null || cart.Products.Count == 0;
            if (lackingProducts)
                return false;
            CheckDiscountCondition(cart);
            if (!_conditionSatisifed)
                return false;
            DiscountProduct(cart);
            //now we remove the affected products
            //the overriden equality operator in product will make sure we remove the ones we need 
            if (this.DiscountApplied)
            {
                ///first remove the products that were part of discount condition
                cart.UnaffectedByDiscount = cart.UnaffectedByDiscount.Except(this.DiscountConditionProducts).ToList();
                ///then remove all discounted products
                cart.UnaffectedByDiscount = cart.UnaffectedByDiscount.Except(this.DiscountedProducts).ToList();
                ///set a flag for products that are part of a condition
                this.DiscountConditionProducts.ForEach(x=>x.IsPartOfDiscountCondition = true);
                

            }
            return this.DiscountApplied;
        }


        /// <summary>
        /// Check if the condition for a discount exists
        /// </summary>
        /// <param name="cart"></param>
        protected abstract void CheckDiscountCondition(ShoppingCart cart);
        /// <summary>
        /// Apply discount. Remove the 
        /// </summary>
        /// <param name="cart"></param>
        protected abstract void DiscountProduct(ShoppingCart cart);

        public virtual void Cancel()
        {

            if (this.DiscountConditionProducts != null)
            {
                this.DiscountConditionProducts.ForEach(x => x.IsPartOfDiscountCondition = false);
                this.DiscountConditionProducts.Clear();
            }
            this.ConditionSatisfied = false;
            if (this.DiscountedProducts != null)
            {
                this.DiscountedProducts.ForEach(x => { x.IsDiscounted = false; x.PriceAfterDiscount = null;  });
                this.DiscountedProducts.Clear();

            }
                

        }

    }
}
