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

    public override Coordinate ReadJson(JsonReader reader, Type objectType, Coordinate existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        switch (reader.TokenType)
        {
            case JsonToken.StartArray:
            {
                // reader.Read();
                int beginDepth = reader.Depth + 1;
                int x = reader.ReadAsInt32()!.Value;
                int y = reader.ReadAsInt32()!.Value;
                while (reader.Depth >= beginDepth && reader.Read())
                {
                }

                return new Coordinate(x, y);
            }
            case JsonToken.StartObject:
            {
                int x = 0;
                int y = 0;
                bool xRead = false;
                bool yRead = false;
                int beginDepth = reader.Depth + 1;
                while (!(xRead && yRead))
                {
                    reader.Read();
                    string propertyName = reader.Value!.ToString()!;

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

    private static void SkipDepth(JsonReader reader, int depth)
    {
        while (reader.Depth >= depth && reader.Read())
        {
        }
    }
}