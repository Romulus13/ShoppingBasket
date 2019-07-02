using System;
using System.Collections.Generic;
using System.Text;
using ShoppingBasket.DiscountModel;
using ShoppingBasket.ProductModel;
using System.Linq;
namespace ShoppingBasket.ShopppingCartModel
{
    public class ShoppingCart<T> : IShoppingCart<T>
        where T: class
    {
        

        private List<Discount<T>> _applicableDiscounts;
        private Decimal _totalPriceBeforDiscount;
  

        private String _cartId;
        /// <summary>
        /// A copy of all products that has its products removed if they are either a condition for a discount or a discounted product
        /// </summary>
        private List<Product> _unaffectedByDiscount;
        public ShoppingCart()
        {
            _products = new List<Product>();
            _unaffectedByDiscount = new List<Product>();
            _applicableDiscounts = new List<Discount<T>>();
            _cartId = Guid.NewGuid().ToString();
        }
        /// <summary>
        /// Total price after discounts
        /// </summary>
        
        public List<Product> Products { get => _products; set => _products = value; }
        public decimal TotalPrice { get => _totalPrice; set => _totalPrice = value; }
        public decimal TotalPriceBeforDiscount { get => _totalPriceBeforDiscount; set => _totalPriceBeforDiscount = value; }
        public List<Product> UnaffectedByDiscount { get => _unaffectedByDiscount; set => _unaffectedByDiscount = value; }
        public string CartId { get => _cartId; set => _cartId = value; }
        public List<Discount<T>> ApplicableDiscounts { get => _applicableDiscounts; set => _applicableDiscounts = value; }

        private Decimal _totalPrice;

        private List<Product> _products;
  

        public void Add(Product product)
        {
            if (product != null && !String.IsNullOrEmpty(product.ProductId) && !this._products.Contains(product))
            {
                Products.Add(product);
                UnaffectedByDiscount.Add(product);
                ApplyDiscounts();
                CalculatePrices();
            }
        }

        public bool AddDiscount(Discount<T> discount)
        {
            ///we will not add a discount that has already been applied
            ///TODO MAybe throw an exception!?
            if (discount == null || discount.IsDiscountApplied())
            {
                return false;
            }

            if (this.ApplicableDiscounts == null)
                this.ApplicableDiscounts = new List<Discount<T>>() { discount };
            else
                this.ApplicableDiscounts.Add(discount);
            ///we call applydiscounts so
            ApplyDiscounts();
            CalculatePrices();
            return true;
        }

        public bool RemoveDiscount(Discount<T> discount)
        {
            if (discount == null || this.ApplicableDiscounts == null || !this.ApplicableDiscounts.Contains(discount))
            {
                return false;
            }

            discount.RemoveAllConditionDiscounts();
            this.ApplicableDiscounts.Remove(discount);
            CalculatePrices();
            return true;
        }


        public void Remove(Product product)
        {

            if (product != null && !String.IsNullOrEmpty(product.ProductId))
            {
                ///if product is in the list for unnaffected by discount, remove it from both lists and adjust the price
                if (this.UnaffectedByDiscount.Contains(product))
                {
                    _products.Remove(product);
                    UnaffectedByDiscount.Remove(product);
                    this.TotalPriceBeforDiscount = product.Price <= this.TotalPriceBeforDiscount ? this.TotalPriceBeforDiscount - product.Price : 0;
                    this._totalPrice -=  product.Price;
                    return;
                }
                else if (this._products.Contains(product))
                {
                    //it is not the most optimal solution from the performance standpoint, however it is a simpler solution
                    //to first cancel all existing discounts, then reapply them
                    CancelDiscounts();
                    _products.Remove(product);
                    ApplyDiscounts();

                }
                CalculatePrices();

            }
        }

        public void CalculatePrices()
        {
            this.TotalPrice = 0m;
            this.TotalPriceBeforDiscount = 0m;
            

            foreach (Product item in this._products)
            {
                this._totalPriceBeforDiscount += item.Price;
                this._totalPrice += item.IsDiscounted && item.PriceAfterDiscount.HasValue? item.PriceAfterDiscount.Value : item.Price;
            }

            Logger.Logger.LogMessageInfo(PrepareShoppingCartOutput());
        }
   
        private string PrepareShoppingCartOutput()
        {
            StringBuilder toReturn = new StringBuilder();
            toReturn.AppendFormat("Shopping cart: {0} \n\r", this.CartId);
            toReturn.AppendLine("Product list:");
            foreach (var prod in this._products)
            {
                toReturn.AppendLine(prod.ToString());
            }
            toReturn.AppendLine("Discount list:");
            foreach (var disc in this.ApplicableDiscounts)
            {
                toReturn.AppendLine(disc.ToString());
            }
            toReturn.AppendLine("TOTAL CART PRICE:" + this.TotalPrice.ToString());
            return toReturn.ToString();
        }

        public void ApplyDiscounts()
        {
            if (this.ApplicableDiscounts == null ||this.ApplicableDiscounts.Count < 1)
            {
                return;
            }

            ///we will skip the discounts that have already been applied
            foreach (Discount<T> discount in this.ApplicableDiscounts.FindAll(x => !x.IsDiscountApplied()))
            {
                discount.ApplyDiscountOnCart(this);
            }
        }
        /// <summary>
        /// Cancel all discounts.
        /// </summary>
        public void CancelDiscounts()
        {
            if (this.ApplicableDiscounts == null || this.ApplicableDiscounts.Count < 1)
            {
                return;
            }

            foreach (var discount in this.ApplicableDiscounts)
            {
                discount.RemoveAllConditionDiscounts();
            }
        }
    }
}
