using ShoppingBasket.ProductModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingBasket.DiscountModel
{
    public class ProductConditionDiscount
    {
        private List<Product> _conditionProducts;
        private List<Product> _discounted;
        private bool _conditionSatisfied;
        //private int _numOfProductsToBuy;
        //private ProductType _typeToBuy;

        public bool DiscountApplied
        {
            get
            {
                if (this.Discounted != null)
                    return this.Discounted.Count > 0;
                else
                    return false;
            }
        }

        public List<Product> ConditionProducts { get => _conditionProducts; set => _conditionProducts = value; }
        public bool ConditionsSatisfied { get => _conditionSatisfied; set => _conditionSatisfied = value; }
        public List<Product> Discounted { get => _discounted; set => _discounted = value; }

        public ProductConditionDiscount()
        {
            this.Discounted = new List<Product>();
            this.ConditionProducts = new List<Product>();

        }


        internal void ResetProductCondition()
        {
            this.ConditionProducts.ForEach(x => x.IsPartOfDiscountCondition = false);
            this.ConditionProducts.Clear();
            this.Discounted.ForEach(x => { x.IsDiscounted = false; x.PriceAfterDiscount = null; });
            this.Discounted.Clear();
            this.ConditionsSatisfied = false;
        }
        


    }
}
