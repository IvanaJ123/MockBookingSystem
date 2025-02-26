using System.Text.Json;
using System.Text.Json.Serialization;

namespace MockBookingSystem.Models
{
    public class Hotel
    {
        [JsonConverter(typeof(NumberToStringConverter))]
        public string HotelCode { get; set; }
        public double Price { get; set; }
    }

    public class NumberToStringConverter : JsonConverter<string>
    {
        public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Convert numbers to strings
            if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetInt32().ToString();
            }

            // Return the string value if it's already a string
            return reader.GetString() ?? string.Empty;
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value);
        }
    }
}
