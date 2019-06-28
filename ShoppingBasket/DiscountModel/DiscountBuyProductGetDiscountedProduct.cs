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
    public class DiscountBuyProductGetSameDiscounted : Discount
    {
        private Tuple<ProductType, int> _hasToBeBought;
        private Tuple<ProductType, int> _toDiscount;

        public DiscountBuyProductGetSameDiscounted(decimal discountPercent, Tuple<ProductType, int> hasToBeBought, Tuple<ProductType, int> toDiscount)
        {
            this.DiscountPercent = discountPercent;
            this._hasToBeBought = hasToBeBought;
            this._toDiscount = toDiscount;
            this.DiscountConditionProducts = new List<Product>();
            this.DiscountedProducts = new List<Product>();
        }

        protected override void CheckDiscountCondition(ShoppingCart cart)
        {
            //in some instances it could happen that a specific discount had its conditions met, however no product for discount was found
            if (this.ConditionSatisfied)
            {
                return;
            }
            ///MAYBE ADD A LISTOF DISCOUNTCONDITIONS, WHICH HAVE TO BE SATISFIED, EAC HAS CONDITION SATISIFIED, AND A LIST OF DISCOUNTED PRODUCTS, MAYBE OVERENGINEERING
            foreach (var prod in cart.UnaffectedByDiscount)
            {
                ///check if so far we have products which are all equal to the products we have to buy  
                ///and we have enough of them in order to get a discount 
                ///the condition this.discountConditionProducts.TrueForAll(x=>x.Type == _hasToBeBought.Item1) has been removed since we adding products of only specific type anyways
                ///it is excessive to check for it here 
                if (this.DiscountConditionProducts != null && this.DiscountConditionProducts.Count == _hasToBeBought.Item2)
                {
                    this.ConditionSatisfied = true;
                   return;
                }
                ///if this product hasn't been considered for a discount
                if (this.DiscountConditionProducts != null && prod.Type == _hasToBeBought.Item1 && this.DiscountConditionProducts.Count < _hasToBeBought.Item2 && !this.DiscountConditionProducts.Contains(prod))
                {
                    this.DiscountConditionProducts.Add(prod);
                }
            }
            ///we clear the conditional products necessary for the discount to be applied
            if (!this.ConditionSatisfied)
            {
                this.DiscountConditionProducts.Clear();
            }
        }

        protected override void DiscountProduct(ShoppingCart cart)
        {

            ///iterate through all of products currently not a part of discount, and currently not in the condtion of
            ///a discount we are trying to apply
            foreach (var product in cart.UnaffectedByDiscount.Except(this.DiscountConditionProducts))
            {
                ///if discountedPRoducts are uninitalized we break to avoid NullException
                ///if we filled the discounted products with the required number of products we also break
                if (this.DiscountedProducts == null || this.DiscountedProducts.Count >= _toDiscount.Item2)
                {
                    break;
                }
                if (product.Type == _toDiscount.Item1 && this.DiscountedProducts.Count < _toDiscount.Item2 && !this.DiscountedProducts.Contains(product))
                {
                    this.DiscountedProducts.Add(product);
                    product.IsDiscounted = true;
                    product.PriceAfterDiscount = product.Price * (1 - this.DiscountPercent);

                }
            }
            

        }
    }
}
