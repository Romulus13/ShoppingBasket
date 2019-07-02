using ShoppingBasket;
using ShoppingBasket.DiscountModel;
using ShoppingBasket.ProductModel;
using ShoppingBasket.ShopppingCartModel;
using System;
using System.Collections.Generic;
using Xunit;

namespace ShoppingBasketTest
{
    public class ShoppingCartTest
    {
        [Theory]
        [ClassData(typeof(ShoppingCartData))]
        public void GetTotalShoppingCartPrice(ShoppingCart<ProductConditionDiscount> cart, decimal totalPriceAfterDiscount)
        {
            cart.CalculatePrices();

            Assert.Equal(totalPriceAfterDiscount, cart.TotalPrice);
        }

        [Fact]
        public void AddProduct_ReturnCount()
        {
            var shoppingCart = new ShoppingCart<ProductConditionDiscount>();
            var product = new Product("Milk", 1.15m, ProductType.MILK);
            shoppingCart.Add(product);
            Assert.Single(shoppingCart.Products);
        }

        [Fact]
        public void AddSameProduct_ReturnCount()
        {
            var shoppingCart = new ShoppingCart<ProductConditionDiscount>();
            var product = new Product("Milk", 1.15m, ProductType.MILK);
            shoppingCart.Add(product);
            shoppingCart.Add(product);
            Assert.Single(shoppingCart.Products);
        }

        [Theory]
        [MemberData(nameof(AddProductData))]
        public void AddProduct_ReturnNewPrice(ShoppingCart<ProductConditionDiscount> cart)
        {
            cart.Add(new Product("Butter", 0.8m, ProductType.BUTTER));
            Assert.Equal(3.25m, cart.TotalPrice);
        }

        [Theory]
        [MemberData(nameof(RemoveProductData))]
        public void RemoveProduct_ReturnNewPrice(ShoppingCart<ProductConditionDiscount> cart)
        {
            Product butter = cart.Products.Find(y => y.Type == ProductType.BUTTER);
            cart.Remove(butter);
            Assert.Equal(2.95m, cart.TotalPrice);
        }

        [Theory]
        [MemberData(nameof(TestDiscountsData))]
        public void RemoveDiscount_ReturnNewPrice(ShoppingCart<ProductConditionDiscount> cart)
        {
            ProductDiscount disc1 = cart.ApplicableDiscounts[0] as ProductDiscount;
            cart.RemoveDiscount(disc1);
            Assert.Equal(4.9m, cart.TotalPrice);
        }

        [Theory]
        [MemberData(nameof(TestDiscountsData))]
        public void AddDiscount_ReturnNewPrice(ShoppingCart<ProductConditionDiscount> cart)
        {

            ProductDiscount disc2 = new ProductDiscount(1m, 1, ProductType.MILK, 1, ProductType.MILK);
            cart.AddDiscount(disc2);
            Assert.Equal(3.25m, cart.TotalPrice);

        }


        

        public static TheoryData<ShoppingCart<ProductConditionDiscount>> AddProductData
        {
            get
            {
                var data = new TheoryData<ShoppingCart<ProductConditionDiscount>>();
                ///initialize discounts
                ProductDiscount disc1 = new ProductDiscount(0.5m, 1, ProductType.BREAD, 2, ProductType.BUTTER);
                ProductDiscount disc2 = new ProductDiscount(1m, 1, ProductType.MILK, 3, ProductType.MILK); 
                ///init shopping cart
                ShoppingCart<ProductConditionDiscount> shoppingCart = new ShoppingCart<ProductConditionDiscount>();
                Product milk1 = new Product("Milk", 1.15m, ProductType.MILK);
                Product butter1 = new Product("Butter", 0.8m, ProductType.BUTTER);
                Product bread1 = new Product("Bread", 1.0m, ProductType.BREAD);
                shoppingCart.Add(milk1);
                shoppingCart.Add(butter1);
                shoppingCart.Add(bread1);

                shoppingCart.AddDiscount(disc1);
                shoppingCart.AddDiscount(disc2);
                data.Add(shoppingCart);
                return data;
            }
        }


        public static TheoryData<ShoppingCart<ProductConditionDiscount>> RemoveProductData
        {
            get
            {
                var data = new TheoryData<ShoppingCart<ProductConditionDiscount>>();
                ///initialize discounts
                ProductDiscount disc1 = new ProductDiscount(0.5m, 1, ProductType.BREAD, 2, ProductType.BUTTER);
                ProductDiscount disc2 = new ProductDiscount(1m, 1, ProductType.MILK, 3, ProductType.MILK);
                ///init shopping cart
                ShoppingCart<ProductConditionDiscount> shoppingCart = new ShoppingCart<ProductConditionDiscount>();
                Product milk1 = new Product("Milk", 1.15m, ProductType.MILK);
                Product butter1 = new Product("Butter", 0.8m, ProductType.BUTTER);
                Product butter2 = new Product("Butter", 0.8m, ProductType.BUTTER);
                Product bread1 = new Product("Bread", 1.0m, ProductType.BREAD);
                shoppingCart.Add(milk1);
                shoppingCart.Add(butter1);
                shoppingCart.Add(butter2);
                shoppingCart.Add(bread1);

                shoppingCart.AddDiscount(disc1);
                shoppingCart.AddDiscount(disc2);
                data.Add(shoppingCart);
                return data;
            }
        }


        public static TheoryData<ShoppingCart<ProductConditionDiscount>> TestDiscountsData
        {
            get
            {
                var data = new TheoryData<ShoppingCart<ProductConditionDiscount>>();
                ///initialize discounts
                ProductDiscount disc1 = new ProductDiscount(0.5m, 1, ProductType.BREAD, 2, ProductType.BUTTER);
                
                ///init shopping cart
                ShoppingCart<ProductConditionDiscount> shoppingCart = new ShoppingCart<ProductConditionDiscount>();
                Product milk1 = new Product("Milk", 1.15m, ProductType.MILK);
                Product milk2 = new Product("Milk", 1.15m, ProductType.MILK);
                Product butter1 = new Product("Butter", 0.8m, ProductType.BUTTER);
                Product butter2 = new Product("Butter", 0.8m, ProductType.BUTTER);
                Product bread1 = new Product("Bread", 1.0m, ProductType.BREAD);
                shoppingCart.Add(milk1);
                shoppingCart.Add(butter1);
                shoppingCart.Add(butter2);
                shoppingCart.Add(bread1);
                shoppingCart.Add(milk2);
                shoppingCart.AddDiscount(disc1);
                
                data.Add(shoppingCart);
                return data;
            }
        }


    }
}
