using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TodoApp.Dtos.Converters;

public class DateOnlyConverter : JsonConverter<DateOnly>
{
    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateOnly.ParseExact(reader.GetString()!, "dd-M-yyyy", CultureInfo.InvariantCulture);
    }

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture));
    }
}