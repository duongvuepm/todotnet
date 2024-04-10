using System.ComponentModel.DataAnnotations;

namespace TodoApp.Dtos.Validators;

public class FutureDateAttribute : ValidationAttribute
{
    private readonly ILogger<FutureDateAttribute> _logger =
        LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<FutureDateAttribute>();

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not DateOnly dueDate)
        {
            return ValidationResult.Success;
        }

        _logger.LogInformation($"Validating due date {dueDate}");

        return dueDate > DateOnly.FromDateTime(DateTime.Now)
            ? ValidationResult.Success
            : new ValidationResult("Due date must be in the future");
    }
}