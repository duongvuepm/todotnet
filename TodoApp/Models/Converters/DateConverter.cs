using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace TodoApp.Models.Converters;

public class DateConverter : ValueConverter<DateOnly, DateTime>
{
    public DateConverter()
        : base(dateOnly => dateOnly.ToDateTime(TimeOnly.MinValue),
            dateTime => DateOnly.FromDateTime(dateTime))
    {
    }
}