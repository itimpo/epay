using DtPay.Models;
using DtPay.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit.Abstractions;

namespace DtPay.Tests
{
    [Collection("DtPayTests")]
    public class CustomerApiTests :
        IClassFixture<WebApplicationFactory<Program>>, 
        IDisposable
    {
        private readonly WebApplicationFactory<Program> _factory;
        private HttpClient _httpClient;
        private readonly ITestOutputHelper _output;
        private StorageService _storage;
        private JsonSerializerOptions _serializeOptions = new()
        {
            WriteIndented = true
        };

        private string[] _firstNames = ["Leia", "Sadie", "Jose", "Sara", "Frank", "Dewey", "Tomas", "Joel", "Lukas", "Carlos"];
        private string[] _lastNames = ["Liberty", "Ray", "Harrison", "Ronan", "Drew", "Powell", "Larsen", "Chan", "Anderson", "Lane"];

        private int _maxId = 0;

        public CustomerApiTests(
            WebApplicationFactory<Program> factory, 
            ITestOutputHelper output)
        {
            _factory = factory; 
            _httpClient = factory.CreateClient();
            _output = output;
            
            _storage = factory.Services.GetRequiredService<StorageService>();
        }

        [Fact]
        public async Task T1ValidCustomersPost()
        {
            /*
             The simulated POST customers requests have following requirements:
                •	Each request should contain at least 2 different customers
                •	Age should be randomized between 10 and 90
                •	ID should be increasing sequentially.
                •	The first names and last names of the Appendix should be used in random combinations
             */

            _storage.Clear();

            var customers = GenerateCustomers(3);

            _output.WriteLine("POST");
            _output.WriteLine(JsonSerializer.Serialize(customers, _serializeOptions));

            var response = await _httpClient.PostAsJsonAsync("/customer", customers);

            _output.WriteLine($"ResponseCode:{response.StatusCode}:Content:{await response.Content.ReadAsStringAsync()}");
            
            Assert.True(response?.IsSuccessStatusCode);

            var result = await GetCustomers();

            Assert.True(result!=null && result.All(c => customers.Contains(c)));
        }

        [Fact]
        public async Task T2InvalidAgeCustomersPost()
        {
            var customer = GenerateCustomers(1).First();
            customer.Age = 15;

            _output.WriteLine("POST");
            _output.WriteLine(JsonSerializer.Serialize(customer, _serializeOptions));

            var response = await _httpClient.PostAsJsonAsync("/customer", new[] { customer });

            _output.WriteLine($"ResponseCode:{response.StatusCode}:Content:{await response.Content.ReadAsStringAsync()}");

            Assert.True(!response?.IsSuccessStatusCode);
        }

        [Fact]
        public async Task T3InvalidIdCustomersPost()
        {
            var customer = GenerateCustomers(1).First();
            //should be already created previously
            customer.Id = 1;

            _output.WriteLine("POST");
            _output.WriteLine(JsonSerializer.Serialize(customer, _serializeOptions));

            var response = await _httpClient.PostAsJsonAsync("/customer", new[] { customer });

            _output.WriteLine($"ResponseCode:{response.StatusCode}:Content:{await response.Content.ReadAsStringAsync()}");

            Assert.True(!response?.IsSuccessStatusCode);
        }

        #region private methods

        private async Task<Customer[]> GetCustomers()
        {
            var result = await _httpClient.GetFromJsonAsync<Customer[]>("/customer");

            _output.WriteLine("GET");
            _output.WriteLine(JsonSerializer.Serialize(result, _serializeOptions));

            //_maxId = (result?.Max(q => q.Id)??-1) + 1;

            return result ?? [];
        }
                
        private Customer[] GenerateCustomers(int count)
        {
            var random = new Random();
            var customers = Enumerable.Range(_maxId, count)
                .ToList()
                .Select(id => new Customer
                {
                    Id = id,
                    FirstName = _firstNames[random.Next(0, _firstNames.Length)],
                    LastName = _lastNames[random.Next(0, _lastNames.Length)],
                    Age = random.Next(10, 90)
                }).ToArray();
            _maxId += count;
            return customers;
        }

        #endregion

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}