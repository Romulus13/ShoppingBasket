# ShoppingBasket
Author: Roman Vazdar

ShoppingBasket is a class library with a xUnit test project that tries to demonstrate a easily plugable library for calculating discounts in shopping carts.

The logging component logs information into the ShoppingBasket\ShoppingBasketTest\bin\Debug\netcoreapp2.2 folder. Specifically the file with the filename "log". A NLog library has been used for logging. With a simple static wrapper class in a seperate project.

When starting to implement this project I went the standard OOP route and tried to mould the domain into specific classes that represent the abstraction of the domain model (Web shop shopping cart).
However there are two other possible routes one might take when trying to represent the discounts applicable on the Shopping Cart with adequate program logic. One would be to take the included simple rules engine in the class DiscountRule.
The rules represented there might describe specific discounts more generically. More precisely one could express the conditions for the discount through the rules described there. Afterwards an action is needed, meaning given a satisfied condtition we have to discount something. Those rules then need to be paired with an action component. Also chaining the conditions/rules means they need to be a part of a collection set up inside each discount. This can then become a more difficult task.

However in order to follow a truly generic approach without trying to model it with sufficiently applicable and representative generic types and constraints, one could use NRules engine. 
The NRules engine is available here https://github.com/NRules/NRules/wiki.


