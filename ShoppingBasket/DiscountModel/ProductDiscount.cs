using ShoppingBasket.ProductModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace ShoppingBasket.DiscountModel
{
    /// <summary>
    /// 
    /// </summary>
    public class ProductDiscount : Discount
    {

        private ProductType _toDiscount;
        private int _numToDiscount;
        private int _numToBeBought;
        private ProductType _toBuy;
        private List<ProductConditionDiscount> _conditionDiscounts;

        public ProductDiscount(decimal discountPercent, int numToDiscount, ProductType toDiscount, int numToBeBought, ProductType toBuy)
        {
            this.DiscountPercent = discountPercent;
            this._numToBeBought = numToBeBought;
            this._toBuy = toBuy;
            this._toDiscount = toDiscount;
            this._numToDiscount = numToDiscount;
            this._conditionDiscounts = new List<ProductConditionDiscount>();
        }

        public override bool ApplyDiscountOnCart(ShoppingCart cart)
        {
            bool lackingProducts = cart == null || cart.Products == null || cart.Products.Count == 0;
            if (lackingProducts)
                return false;
            CheckDiscountCondition(cart);
            if (this._conditionDiscounts.Count == 0)
                return false;
            DiscountProduct(cart);
            ///first we cancel all discounts that were not applied. MEaning they only had a satisfied conditon
            CancelNonDiscountedConditions();
            ///then we remove all products from the unaffected list of the cart that have their discounts applied and conditions satisfied
            foreach (ProductConditionDiscount prodCondDisc in this._conditionDiscounts.Where(y => y.DiscountApplied))
            {
                cart.UnaffectedByDiscount.RemoveAll(unnaffectedPRod => prodCondDisc.ConditionProducts.Contains(unnaffectedPRod) || prodCondDisc.Discounted.Contains(unnaffectedPRod));
            }
            return IsDiscountApplied();
        }

        public override void RemoveAllConditionDiscounts()
        {
            if (this._conditionDiscounts != null)
            {
                this._conditionDiscounts.ForEach(x => x.ResetProductCondition()); ;
                this._conditionDiscounts.Clear();
            }
        }

        public override bool IsDiscountApplied()
        {
            return this._conditionDiscounts.Any(x=>x.DiscountApplied);
        }

        /// <summary>
        /// Method which checks if there are product discount which satisfy the condition.
        /// </summary>
        /// <param name="cart"></param>
        protected override void CheckDiscountCondition(ShoppingCart cart)
        {
            var temp = new ProductConditionDiscount();
            foreach (var prod in cart.UnaffectedByDiscount)
            {
                ///check if the product is a condition for the discount
                if ( prod.Type == _toBuy && temp.ConditionProducts.Count < _numToBeBought && !temp.ConditionProducts.Contains(prod))
                {
                    temp.ConditionProducts.Add(prod);
                }
                if (temp.ConditionProducts.Count == _numToBeBought)
                {
                    temp.ConditionsSatisfied = true;
                    this._conditionDiscounts.Add(temp);
                    temp = new ProductConditionDiscount();
                }
            }
            
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="cart"></param>
        protected override void DiscountProduct(ShoppingCart cart)
        {
            foreach (ProductConditionDiscount item in this._conditionDiscounts)
            {
                var candidatesForDiscount = cart.UnaffectedByDiscount.Except(item.ConditionProducts);
                ///iterate through all of products currently not a part of discount, and currently not in the condition of
                ///a discount we are trying to apply
                foreach (var product in candidatesForDiscount)
                {
                    ///if we filled the discounted products with the required number of products we also break
                    if (item.Discounted.Count >= _numToDiscount)
                    {
                        break;
                    }
                    ///find the prodduct for discount and apply the discount on its price
                    if (product.Type == _toDiscount && item.Discounted.Count < _numToDiscount && !item.Discounted.Contains(product))
                    {
                        item.Discounted.Add(product);
                        product.IsDiscounted = true;
                        product.PriceAfterDiscount = product.Price * (1 - this.DiscountPercent);

                    }
                }
                
            }
        }


        private void CancelNonDiscountedConditions()
        {
            var nonDiscounted = this._conditionDiscounts.Where(x => !x.DiscountApplied);
            if (nonDiscounted != null)
            {
                nonDiscounted.ToList().ForEach(y => { y.ResetProductCondition(); this._conditionDiscounts.Remove(y); });
            }
        }
    }
}
