using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HueCLI.Logic
{
    public class GamutJsonConverter : JsonConverter<HueBridgeLightColorGamut>
    {
        public override HueBridgeLightColorGamut Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
                {
                    JsonElement root = doc.RootElement;

                    if (root.GetArrayLength() == 3) {
                        var red = new XYPoint(root[0][0].GetDouble(), root[0][1].GetDouble());
                        var green = new XYPoint(root[1][0].GetDouble(), root[1][1].GetDouble());
                        var blue = new XYPoint(root[2][0].GetDouble(), root[2][1].GetDouble());

                        return new HueBridgeLightColorGamut(red, green, blue);
                    }
                }
            }
            
            return null;
        }

        public override void Write(Utf8JsonWriter writer, HueBridgeLightColorGamut value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}