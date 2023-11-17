using DtPay.Models;
using System.Text.Json;

namespace DtPay.Services;

internal class CustomerComparer : IComparer<Customer>
{
    public int Compare(Customer x, Customer y)
    {
        var compareResult = -y.LastName.CompareTo(x.LastName);
        if (compareResult == 0)
        {
            compareResult = -y.FirstName.CompareTo(x.FirstName);
        }

        return compareResult;
    }
}

public class StorageService
{
    private readonly string _filePath = string.Empty;
    private readonly SortedSet<Customer> _customerSet = new(new CustomerComparer());
    private readonly ILogger<StorageService> _logger;
    private static readonly object lockObject = new();

    public Customer[] Customers => _customerSet.ToArray();

    public StorageService(ILogger<StorageService> logger, IWebHostEnvironment host)
    {
        //load file on create singleton
        _filePath = $"{host.ContentRootPath}\\data.json";
        _logger = logger;
        
        ReadDataFile();
    }

    /// <summary>
    /// Checks if already exists customer with specific id 
    /// </summary>
    /// <param name="id">Id to check</param>
    /// <returns>True if id is unique</returns>
    public bool IsIdUnique(int id)
    {
        return !_customerSet.Any(q => q.Id == id);
    }

    /// <summary>
    /// Clears storage
    /// </summary>
    public void Clear()
    {
        lock (lockObject)
        {
            _customerSet.Clear();
            WriteDataFile();
        }
    }

    /// <summary>
    /// Adds customers to storage
    /// </summary>
    /// <param name="customers">Array of customers</param>
    public void AddCustomers(Customer[] customers)
    {
        lock (lockObject)
        {
            foreach (Customer customer in customers)
            {
                _customerSet.Add(customer);
            }
            WriteDataFile();
        }
    }

    private void ReadDataFile()
    {
        try
        {
            var data = File.ReadAllBytes(_filePath);

            if (data.Length > 0)
            {
                try
                {
                    var customers = JsonSerializer.Deserialize<Customer[]>(data) ?? [];
                    foreach (var customer in customers)
                    {
                        _customerSet.Add(customer);
                    }
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"Error deserializing JSON: {ex.Message}");
                }
            }
            else
            {
                _logger.LogError("File is empty.");
            }
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Exception on file read.");
        }
    }

    private void WriteDataFile()
    {
        try
        {
            var data = JsonSerializer.Serialize(_customerSet);
            File.WriteAllText(_filePath, data);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Exception on file write.");
        }
    }
}
