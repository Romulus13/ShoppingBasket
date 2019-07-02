using System;
using System.Collections.Generic;
using System.Text;

using ShoppingBasket.DiscountModel;
using ShoppingBasket.ProductModel;
using ShoppingBasket.ShopppingCartModel;
using Xunit;

namespace ShoppingBasketTest
{
    public class ShoppingCartData : TheoryData<ShoppingCart<ProductConditionDiscount>, decimal>
    {
        
        public ShoppingCartData()
        {
           

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
            Add(shoppingCart, 2.95m);

            ShoppingCart<ProductConditionDiscount> shoppingCart2 = new ShoppingCart<ProductConditionDiscount>();
            Product butter2 = new Product("Butter", 0.8m, ProductType.BUTTER);
            Product bread2 = new Product("Bread", 1.0m, ProductType.BREAD);
            Product butter3 = new Product("Butter", 0.8m, ProductType.BUTTER);
            Product bread3 = new Product("Bread", 1.0m, ProductType.BREAD);
            shoppingCart2.Add(butter2);
            shoppingCart2.Add(butter3);
            shoppingCart2.Add(bread3);
            shoppingCart2.Add(bread2);
            disc1 = new ProductDiscount(0.5m, 1, ProductType.BREAD,2, ProductType.BUTTER);
            disc2 = new ProductDiscount(1m, 1, ProductType.MILK, 3, ProductType.MILK);
            shoppingCart2.AddDiscount(disc1);
            shoppingCart2.AddDiscount(disc2);
            Add(shoppingCart2, 3.1m);
            ShoppingCart<ProductConditionDiscount> shoppingCart3 = new ShoppingCart<ProductConditionDiscount>();
            Product milk1_cart3 = new Product("Milk", 1.15m, ProductType.MILK);
            Product milk2_cart3 = new Product("Milk", 1.15m, ProductType.MILK);
            Product milk3_cart3 = new Product("Milk", 1.15m, ProductType.MILK);
            Product milk4_cart3 = new Product("Milk", 1.15m, ProductType.MILK);
            shoppingCart3.Add(milk1_cart3);
            shoppingCart3.Add(milk2_cart3);
            shoppingCart3.Add(milk3_cart3);
            shoppingCart3.Add(milk4_cart3);
            disc1 = new ProductDiscount(0.5m, 1, ProductType.BREAD, 2, ProductType.BUTTER);
            disc2 = new ProductDiscount(1m, 1, ProductType.MILK, 3, ProductType.MILK);
            shoppingCart3.AddDiscount(disc1);
            shoppingCart3.AddDiscount(disc2);
            Add(shoppingCart3, 3.45m);
            ShoppingCart<ProductConditionDiscount> shoppingCart4 = new ShoppingCart<ProductConditionDiscount>();
            Product milk1_cart4 = new Product("Milk", 1.15m, ProductType.MILK);
            Product milk2_cart4 = new Product("Milk", 1.15m, ProductType.MILK);
            Product milk3_cart4 = new Product("Milk", 1.15m, ProductType.MILK);
            Product milk4_cart4 = new Product("Milk", 1.15m, ProductType.MILK);
            Product milk5_cart4 = new Product("Milk", 1.15m, ProductType.MILK);
            Product milk6_cart4 = new Product("Milk", 1.15m, ProductType.MILK);
            Product milk7_cart4 = new Product("Milk", 1.15m, ProductType.MILK);
            Product milk8_cart4 = new Product("Milk", 1.15m, ProductType.MILK);
            Product butter1_cart4 = new Product("Butter", 0.8m, ProductType.BUTTER);
            Product bread1_cart4 = new Product("Bread", 1.0m, ProductType.BREAD);
            Product butter2_cart4 = new Product("Butter", 0.8m, ProductType.BUTTER);
            shoppingCart4.Add(milk1_cart4);
            shoppingCart4.Add(milk2_cart4);
            shoppingCart4.Add(milk3_cart4);
            shoppingCart4.Add(milk5_cart4);
            shoppingCart4.Add(milk4_cart4);
            shoppingCart4.Add(milk6_cart4);
            shoppingCart4.Add(milk7_cart4);
            shoppingCart4.Add(milk8_cart4);
            shoppingCart4.Add(butter1_cart4);
            shoppingCart4.Add(bread1_cart4);
            shoppingCart4.Add(butter2_cart4);
            disc1 = new ProductDiscount(0.5m, 1, ProductType.BREAD, 2, ProductType.BUTTER);
            disc2 = new ProductDiscount(1m, 1, ProductType.MILK, 3, ProductType.MILK);
            shoppingCart4.AddDiscount(disc1);
            shoppingCart4.AddDiscount(disc2);
            Add(shoppingCart4, 9m);
        }

      




    }
}
