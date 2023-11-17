using DtPay.Services;
using System.ComponentModel.DataAnnotations;

namespace DtPay.Validation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class UniqueIdAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        var storage = validationContext.GetRequiredService<StorageService>();
        // Ensure that the value is not null and is of type int
        if (value is int id)
        {
            // Check if the ID has been used before use injected Storage Service to check
            if (!storage.IsIdUnique(id))
            {
                return new ValidationResult("The ID has already been used.");
            }
        }

        return ValidationResult.Success!;
    }
}
