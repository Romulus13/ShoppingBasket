using System;
using System.Collections.Generic;
using ShoppingBasket.DiscountModel;
namespace ShoppingBasket.ProductModel

{
    public class Product
    {

        private ProductType _type;
        private string _name;
        private string _productId;
        private decimal _price;
        private decimal? _priceAfterDiscount;
        //private Discount _discount;
        private bool isPartOfDiscountCondition;
        private bool isDiscounted;
        public String Name { get => _name; set => _name = value; }
        public String ProductId { get => _productId; }

        public decimal Price { get => _price; set => _price = value; }

        public decimal? PriceAfterDiscount { get => _priceAfterDiscount; set => _priceAfterDiscount = value; }
       // public Discount Discount { get => _discount; set => _discount = value; }
        public ProductType Type { get => _type; set => _type = value; }
        public bool IsPartOfDiscountCondition { get => isPartOfDiscountCondition; set => isPartOfDiscountCondition = value; }
        public bool IsDiscounted { get => isDiscounted; set => isDiscounted = value; }

        public Product()
        {
            this._productId = Guid.NewGuid().ToString();
        }

        public Product(string name,  decimal price, ProductType type)
        {
            this._name = name;
            this._price = price;
            this._productId = Guid.NewGuid().ToString();
            this._type = type;

        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return obj is Product product &&
                   _productId == product._productId;
        }

        public override int GetHashCode()
        {
            return 378675680 + EqualityComparer<string>.Default.GetHashCode(_productId);
        }
    }
}
