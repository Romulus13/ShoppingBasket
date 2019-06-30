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
        private String _id;
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
        public string Id { get => _id;  }

        public ProductConditionDiscount()
        {
            this.Discounted = new List<Product>();
            this.ConditionProducts = new List<Product>();
            this._id = Guid.NewGuid().ToString();
        }


        internal void ResetProductCondition()
        {
            this.ConditionProducts.ForEach(x => x.IsPartOfDiscountCondition = false);
            this.ConditionProducts.Clear();
            this.Discounted.ForEach(x => { x.IsDiscounted = false; x.PriceAfterDiscount = null; });
            this.Discounted.Clear();
            this.ConditionsSatisfied = false;
        }

        public override string ToString()
        {
            StringBuilder toReturn = new StringBuilder("Product condition discount " + this.Id );
            toReturn.AppendLine("Products that are part of condition for discount:");
            foreach (var condProd in _conditionProducts)
            {
                toReturn.AppendLine(condProd.ToString());
            }
            toReturn.AppendLine("Products that are discounted:");
            foreach (var discProd in _discounted)
            {
                toReturn.AppendLine(discProd.ToString());
            }

            return base.ToString();
        }

    }
}
