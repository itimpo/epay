using DtPay.Validation;
using System.ComponentModel.DataAnnotations;

namespace DtPay.Models;

public record struct Customer
{
    public Customer(){}

    [UniqueId(ErrorMessage = "The ID has already been used.")]
    public int Id { get; set; }
    [Required]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    public string LastName { get; set; } = string.Empty;
    [Required, Range(18, int.MaxValue, ErrorMessage = "Age should be above 18")]
    public int Age { get; set; } = 18;
}
