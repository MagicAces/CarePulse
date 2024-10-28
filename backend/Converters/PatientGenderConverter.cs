using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using backend.Enums;

namespace backend.Converters
{
    public class PatientGenderConverter : JsonConverter<Gender>
    {
        public override Gender Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                string value = reader.GetString();
                return Enum.TryParse<Gender>(value, true, out var gender) ? gender : throw new JsonException();
            }
            return default;
        }

        public override void Write(Utf8JsonWriter writer, Gender value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}