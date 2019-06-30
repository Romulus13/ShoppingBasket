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
        public void GetTotalShoppingCartPrice(ShoppingCart cart, decimal totalPriceAfterDiscount)
        {
            cart.CalculatePrices();

            Assert.Equal(totalPriceAfterDiscount, cart.TotalPrice);
        }

        [Fact]
        public void AddProduct_ReturnCount()
        {
            var shoppingCart = new ShoppingCart();
            var product = new Product("Milk", 1.15m, ProductType.MILK);
            shoppingCart.Add(product);
            Assert.Single(shoppingCart.Products);
        }

        [Fact]
        public void AddSameProduct_ReturnCount()
        {
            var shoppingCart = new ShoppingCart();
            var product = new Product("Milk", 1.15m, ProductType.MILK);
            shoppingCart.Add(product);
            shoppingCart.Add(product);
            Assert.Single(shoppingCart.Products);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void AddProduct_ReturnNewPrice(ShoppingCart cart)
        {
            cart.Add(new Product("Butter", 0.8m, ProductType.BUTTER));
            cart.CalculatePrices();
            Assert.Equal(3.25m, cart.TotalPrice);
        }

        public static TheoryData<ShoppingCart> Data
        {
            get
            {
                var data = new TheoryData<ShoppingCart>();
                ///initialize discounts
                ProductDiscount disc1 = new ProductDiscount(0.5m, 1, ProductType.BREAD, 2, ProductType.BUTTER);
                ProductDiscount disc2 = new ProductDiscount(1m, 1, ProductType.MILK, 3, ProductType.MILK); 
                ///init shopping cart
                ShoppingCart shoppingCart = new ShoppingCart();
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


        ///test calculkate total prices
        ///



        ///test remove discount
        ///

        ///test add discount
        ///

        ///test add produict
        ///


        ///test removeproduct
        ///

        ///test cancel discount
        ///

        ///test apply discount
        ///




    }
}
