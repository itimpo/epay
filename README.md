
 # ePay Coding Test

The solution contains two projects and tests for them.

## Denominator Project

The Denominator project is a library that provides the **DenominatorService**. This service can be used to find all possible combinations for a specific amount.

You can run all tests in the **Denominator.Tests** project to validate the required functionality of the service.

## DtPay Project

The DtPay project is a web API project that manages customers. When you run this project, you can access the Swagger page to check its functionality.

The project use the **/data.json** file as storage, which contains JSON objects of customers. It is loaded on the application's start. Inside the Storage class, a SortedSet collection is used for efficient handling of customer data.

To validate Customer entities during data submission, validation attributes are used, and a custom attribute is implemented to ensure a unique ID.

Tests for the API are available in the **DtPay.Tests** project. Run them to ensure the correctness of the API's functionalities.


*Please note that this is a test project, and the architecture applied may not be suitable for Enterprise applications. Consider the following notes on the design.*
