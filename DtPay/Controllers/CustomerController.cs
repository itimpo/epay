using DtPay.Models;
using DtPay.Services;
using Microsoft.AspNetCore.Mvc;

namespace DtPay.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomerController(StorageService storage, ILogger<CustomerController> logger) 
    : ControllerBase
{
    private readonly ILogger<CustomerController> _logger = logger;
    private readonly StorageService _storage = storage;

    /// <summary>
    /// Returns array of customers from storage
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public Customer[] GetCustomers()
    {
        return _storage.Customers;
    }

    /// <summary>
    /// Add customers to the storage
    /// </summary>
    /// <param name="customers">Array of customers</param>
    /// <returns>Ok or BadRequest if validations is failed</returns>
    [HttpPost]
    public IActionResult PostCustomers([FromBody] Customer[] customers)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        //save to storage
        _storage.AddCustomers(customers);

        return Ok();
    }
}
