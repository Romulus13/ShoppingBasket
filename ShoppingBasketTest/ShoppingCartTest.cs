using ShoppingBasket;
using ShoppingBasket.ProductModel;
using System;
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
