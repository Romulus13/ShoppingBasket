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
        /// <summary>
        /// Method RemoveAllConditionDiscounts removes product condition disocunts.
        /// Before the removal all products are reset to their starting state (e.g., no price after discount, all flags set to false)
        /// </summary>
        public override void RemoveAllConditionDiscounts()
        {
            if (this._conditionDiscounts != null)
            {
                this._conditionDiscounts.ForEach(x => x.ResetProductCondition()); ;
                this._conditionDiscounts.Clear();
            }
        }
        /// <summary>
        /// Checks if any condition discounts are applied
        /// </summary>
        /// <returns>False if discounts conditions haven't been met.</returns>
        public override bool IsDiscountApplied()
        {
            return this._conditionDiscounts.Any(x=>x.DiscountApplied);
        }

        /// <summary>
        /// Method which checks if there are product discounts which satisfy the condition.
        /// </summary>
        /// <param name="cart"></param>
        protected override void CheckDiscountCondition(ShoppingCart cart)
        {
            //create the starting product condition discount
            var temp = new ProductConditionDiscount();
            foreach (var prod in cart.UnaffectedByDiscount)
            {
                ///check if the product is necessary to apply the discount
                if ( prod.Type == _toBuy && temp.ConditionProducts.Count < _numToBeBought && !temp.ConditionProducts.Contains(prod))
                {
                    temp.ConditionProducts.Add(prod);

                }
                ///if we reached the number ofproducts needed to apply the discount we add it the list and set the flag
                if (temp.ConditionProducts.Count == _numToBeBought)
                {
                    temp.ConditionsSatisfied = true;
                    this._conditionDiscounts.Add(temp);
                    temp = new ProductConditionDiscount();
                }
            }
            
        }
        /// <summary>
        ///Method that goes through all the discount whose conditions have been met and applies the discount on the products that meet the requirements.
        /// </summary>
        /// <param name="cart"></param>
        protected override void DiscountProduct(ShoppingCart cart)
        {
            foreach (ProductConditionDiscount item in this._conditionDiscounts)
            {
                var candidatesForDiscount = cart.UnaffectedByDiscount.Except(item.ConditionProducts);
                ///iterate through all of products currently "free", meaning they are not discounted or a conidtion  for a discount
                ///also it is needed to remove the products that are the condition for a discount being checked for application
                foreach (var product in candidatesForDiscount)
                {
                    ///if the discounted products number has been met, break from the loop
                    if (item.Discounted.Count >= _numToDiscount)
                    {
                        break;
                    }
                    ///find the product for discount and apply the discount on its price
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

        public override string ToString()
        {
            StringBuilder toReturn = new StringBuilder( String.Format("Product discount: {0}, product type to buy: {1}, quantity to buy: {2}, product type to discount {3}, quantity to discount  {4} ", this.Name, this._toBuy.ToString(), this._numToBeBought.ToString(), this._toDiscount.ToString(), this._numToDiscount.ToString()));
            foreach (ProductConditionDiscount item in this._conditionDiscounts)
            {
                toReturn.AppendLine(item.ToString());
            }

            return toReturn.ToString();
        }


    }
}
