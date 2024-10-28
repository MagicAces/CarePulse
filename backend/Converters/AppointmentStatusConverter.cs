using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using backend.Enums;

namespace backend.Converters
{
    public class AppointmentStatusConverter : JsonConverter<AppointmentStatus>
    {
        public override AppointmentStatus Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                string value = reader.GetString();
                return Enum.TryParse<AppointmentStatus>(value, true, out var role) ? role : throw new JsonException();
            }
            return default;
        }

        public override void Write(Utf8JsonWriter writer, AppointmentStatus value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}