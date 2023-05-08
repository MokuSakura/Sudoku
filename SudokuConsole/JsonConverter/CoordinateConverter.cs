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
        switch (reader.TokenType)
        {
            case JsonToken.StartArray:
            {
                // reader.Read();
                Int32 beginDepth = reader.Depth + 1;
                Int32 x = reader.ReadAsInt32()!.Value;
                Int32 y = reader.ReadAsInt32()!.Value;
                while (reader.Depth >= beginDepth && reader.Read())
                {
                }

                return new Coordinate(x, y);
            }
            case JsonToken.StartObject:
            {
                Int32 x = 0;
                Int32 y = 0;
                Boolean xRead = false;
                Boolean yRead = false;
                Int32 beginDepth = reader.Depth + 1;
                while (!(xRead && yRead))
                {
                    reader.Read();
                    String propertyName = reader.Value!.ToString()!;

                    if (String.Equals(propertyName, "X", StringComparison.OrdinalIgnoreCase))
                    {
                        x = reader.ReadAsInt32() ?? 0;
                        xRead = true;
                    }
                    else if (String.Equals(propertyName, "Y", StringComparison.OrdinalIgnoreCase))
                    {
                        y = reader.ReadAsInt32() ?? 0;
                        yRead = true;
                    }
                    else
                    {
                        reader.Skip();
                    }
                }

                SkipDepth(reader, beginDepth);
                return new Coordinate(x, y);
            }
            default:
                return existingValue;
        }
    }

    private static void SkipDepth(JsonReader reader, Int32 depth)
    {
        while (reader.Depth >= depth && reader.Read())
        {
        }
    }
}