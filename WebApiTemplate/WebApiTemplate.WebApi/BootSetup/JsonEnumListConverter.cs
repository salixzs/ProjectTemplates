using System.Text.Json.Serialization;
using System.Text.Json;

namespace WebApiTemplate.WebApi.BootSetup;

public class JsonEnumListConverter<T> : JsonConverter<List<T>> where T : struct
{
    public override List<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var enumList = new List<T>();
        if (reader.TokenType == JsonTokenType.StartArray)
        {
            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
            {
                if (reader.TokenType == JsonTokenType.String && Enum.TryParse(reader.GetString(), out T enumItem))
                {
                    enumList.Add(enumItem);
                }
            }
        }

        return enumList;
    }

    public override void Write(Utf8JsonWriter writer, List<T> value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
