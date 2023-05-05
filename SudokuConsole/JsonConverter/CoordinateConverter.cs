using log4net;
using MokuSakura.Sudoku.Core.Coordination;
using Newtonsoft.Json;

namespace MokuSakura.SudokuConsole.JsonConverter;

public class CoordinateConverter : JsonConverter<Coordinate>
{
    private static ILog Log { get; } = LogManager.GetLogger(typeof(CoordinateConverter));

    public override void WriteJson(JsonWriter writer, Coordinate value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.X);
        writer.WriteValue(value.Y);
        writer.WriteEndArray();
    }

    public override Coordinate ReadJson(JsonReader reader, Type objectType, Coordinate existingValue, Boolean hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.StartArray)
        {
            // reader.Read();
            Int32 x = reader.ReadAsInt32() ?? 0;
            Int32 y = reader.ReadAsInt32() ?? 0;

            Int32 depth = reader.Depth;
            while (reader.Read() && (depth <= reader.Depth))
            {
            }

            return new Coordinate(x, y);
        }

        if (reader.TokenType == JsonToken.StartObject)
        {
            Int32 x = 0;
            Int32 y = 0;
            reader.Read();
            Boolean xRead = false;
            Boolean yRead = false;
            while (!(xRead && yRead))
            {
                String propertyName = reader.Value!.ToString()!;
                Int32 value = reader.ReadAsInt32() ?? 0;
                if (String.Equals(propertyName, "X", StringComparison.OrdinalIgnoreCase))
                {
                    x = value;
                    xRead = true;
                }
                else if (String.Equals(propertyName, "Y", StringComparison.OrdinalIgnoreCase))
                {
                    y = value;
                    yRead = true;
                }

                reader.Read();
            }

            if (reader.TokenType != JsonToken.EndObject)
            {
                Int32 depth = reader.Depth;

                while (reader.Read() && (depth <= reader.Depth))
                {
                }
            }

            return new Coordinate(x, y);
        }

        return existingValue;
    }
}