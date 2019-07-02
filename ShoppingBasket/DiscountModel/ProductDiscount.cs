using ShoppingBasket.ProductModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ShoppingBasket.ShopppingCartModel;


namespace ShoppingBasket.DiscountModel
{
    /// <summary>
    /// 
    /// </summary>
    public class ProductDiscount : Discount<ProductConditionDiscount>
    {

        private readonly ProductType _toDiscount;
        private readonly int _numToDiscount;
        private readonly int _numToBeBought;
        private readonly ProductType _toBuy;


        public ProductDiscount(decimal discountPercent, int numToDiscount, ProductType toDiscount, int numToBeBought, ProductType toBuy)
        {
            this.DiscountPercent = discountPercent;
            this._numToBeBought = numToBeBought;
            this._toBuy = toBuy;
            this._toDiscount = toDiscount;
            this._numToDiscount = numToDiscount;
            this.ConditionDiscounts = new List<ProductConditionDiscount>();
        }

        public override bool ApplyDiscountOnCart(ShoppingCart<ProductConditionDiscount> cart)
        {
            try
            {
                bool lackingProducts = cart == null || cart.Products == null || cart.Products.Count == 0;
                if (lackingProducts)
                    return false;
                bool discountApplied = false;
                do
                {
                    var condition = FindDiscountCondition(cart);
                    DiscountProduct(cart, condition);
                    discountApplied = condition.DiscountApplied;
                    if (discountApplied)
                    {
                        cart.UnaffectedByDiscount.RemoveAll(unnaffectedPRod => condition.ConditionItems.Contains(unnaffectedPRod) || condition.Discounted.Contains(unnaffectedPRod));
                    }
                } while (discountApplied);
                /// cancel all discounts that were not applied. MEaning they only had a satisfied conditon
                CancelNonDiscountedConditions();
            }
            catch (Exception e)
            {
                Logger.Logger.LogException(e, "Error in applying discount to a cart!");
                
            }
            return IsDiscountApplied();
        }
        /// <summary>
        /// Method RemoveAllConditionDiscounts removes product condition disocunts.
        /// Before the removal all products are reset to their starting state (e.g., no price after discount, all flags set to false)
        /// </summary>
        public override void RemoveAllConditionDiscounts()
        {
            if (this.ConditionDiscounts != null)
            {
                this.ConditionDiscounts.ForEach(x => x.ResetProductCondition()); ;
                this.ConditionDiscounts.Clear();
            }
        }
        /// <summary>
        /// Checks if any condition discounts are applied
        /// </summary>
        /// <returns>False if discounts conditions haven't been met.</returns>
        public override bool IsDiscountApplied()
        {
            return this.ConditionDiscounts.Any(x => x.DiscountApplied);
        }

        /// <summary>
        /// Method which checks if there are product discounts which satisfy the condition.
        /// </summary>
        /// <param name="cart"></param>
        protected override ProductConditionDiscount FindDiscountCondition(ShoppingCart<ProductConditionDiscount> cart)
        {
            //create the starting product condition discount
            var temp = new ProductConditionDiscount();
            foreach (var prod in cart.UnaffectedByDiscount)
            {
                ///check if the product is necessary to apply the discount
                if (prod.Type == _toBuy && temp.ConditionItems.Count < _numToBeBought && !temp.ConditionItems.Contains(prod) && !prod.IsDiscounted && !prod.IsPartOfDiscountCondition)
                {
                    temp.ConditionItems.Add(prod);

                }
                ///if we reached the number ofproducts needed to apply the discount we add it to the list and set the flag
                if (temp.ConditionItems.Count == _numToBeBought)
                {
                    temp.ConditionSatisfied = true;
                    this.ConditionDiscounts.Add(temp);
                    break;
                }
            }
            return temp;
        }
        /// <summary>
        ///Method that goes through all the discount whose conditions have been met and applies the discount on the products that meet the requirements.
        /// </summary>
        /// <param name="cart"></param>
        protected override void DiscountProduct(ShoppingCart<ProductConditionDiscount> cart, ProductConditionDiscount condition)
        {
            if (cart == null || condition == null || !condition.ConditionSatisfied)
            {
                return;
            }
            var candidatesForDiscount = cart.UnaffectedByDiscount.Except(condition.ConditionItems);
            ///iterate through all of products currently "free", meaning they are not discounted or a conidtion  for a discount
            ///also it is needed to remove the products that are the condition for a discount being checked for application
            foreach (var product in candidatesForDiscount)
            {
                ///if the discounted products number has been met, break from the loop
                if (condition.Discounted.Count >= _numToDiscount)
                {
                    break;
                }
                ///find the product for discount and apply the discount on its price
                if (product.Type == _toDiscount && condition.Discounted.Count < _numToDiscount && !condition.Discounted.Contains(product) && !product.IsDiscounted)
                {
                    condition.Discounted.Add(product);
                    product.IsDiscounted = true;
                    product.PriceAfterDiscount = product.Price * (1 - this.DiscountPercent);

                }
            }


        }


        private void CancelNonDiscountedConditions()
        {
            var nonDiscounted = this.ConditionDiscounts.Where(x => !x.DiscountApplied);
            if (nonDiscounted != null)
            {
                nonDiscounted.ToList().ForEach(y => { y.ResetProductCondition(); this.ConditionDiscounts.Remove(y); });
            }
        }

        public override string ToString()
        {
            StringBuilder toReturn = new StringBuilder(String.Format("Product discount: {0},\t product type to buy: {1},\t quantity to buy: {2},\t product type to discount {3},\t quantity to discount  {4} ", this.Name, this._toBuy.ToString(), this._numToBeBought.ToString(), this._toDiscount.ToString(), this._numToDiscount.ToString()));
            foreach (ProductConditionDiscount item in this.ConditionDiscounts)
            {
                toReturn.AppendLine(item.ToString());
            }

            return toReturn.ToString();
        }


    }
}
