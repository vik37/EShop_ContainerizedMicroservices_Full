using JsonException = Newtonsoft.Json.JsonException;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace EShop.Web.MVC.Models.Converters;

public class NumberToStringConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
        => objectType == typeof(int) || objectType == typeof(string);

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if(reader.TokenType == JsonToken.Integer)
            return reader.Value.ToString();
        if (reader.TokenType == JsonToken.String)
            return reader.Value;
        else
            throw new JsonException($"Wrong JSON Reader Value Type - {reader.ValueType} | -> Json Reader Value should be Number type or {typeof(string)}");
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        if(value is int intValue)
            writer.WriteValue(intValue.ToString());
        else if(value is string stringValue)
            writer.WriteValue(stringValue);
        else
            throw new JsonException($"Wrong JSON Writer Value Type - {value.GetType()} | -> Json Writer Value should be Number type or {typeof(string)}");
    }
}