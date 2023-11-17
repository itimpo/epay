# ePay Coding Test

The solution contains 2 projects and test for them

## Denominator Project 
It's a library with **DenominatorService** that can be used to find all possible combinations for specific amout.
You can run all test in project **Denominator.Test** to proof required functionality of the service.

## DtPay Project
It's a web api project that works with Customers
When you run this project you can see swagger page where you can check functionality.
I used **/data.json** file as storage, it contains json objects of customers, and it is loaded on start of application.
To avoid sorting and to improve performance I used SortedSet collection inside Storage class.
To validate Customer entities on data submit I used validation attributes and also implemented custom attribute to check unique id.
Also, I wrote tests for Api, you can run them in **DtPay.Tests** project

*Please note that this is a test project and I have not applied the architecture that I would use for Enterprise applications.  

  
 