using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonRpg
{
    public class ElementalTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ElementalType) ||
                objectType == typeof(ElementalType?);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            bool isNullable = objectType == typeof(ElementalType?);

            ElementalType? parsedVal = null;
            if (reader.Value != null)
            {
                // Be lenient in what values we accept; parse string or numerical values here
                if (reader.Value is string)
                {
                    parsedVal = ParseElementalType((string)reader.Value);
                }
                else
                {
                    throw new JsonException("Unexpected value " + reader.Value + " for JSON field " + reader.Path);
                }
            }

            if (!parsedVal.HasValue)
            {
                if (isNullable)
                {
                    return null;
                }

                throw new ArgumentNullException("Expected a value for non-nullable JSON field: " + reader.Path);
            }

            if (isNullable)
            {
                return parsedVal;
            }
            else
            {
                return parsedVal.Value;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            string stringVal = null;

            if (value == null)
            {
            }
            else if (value is ElementalType)
            {
                stringVal = PrintElementalType((ElementalType)value);
            }
            else if (value is ElementalType? && ((ElementalType?)value).HasValue)
            {
                stringVal = PrintElementalType(((ElementalType?)value).Value);
            }

            if (stringVal != null)
            {
                writer.WriteValue(stringVal);
            }
            else
            {
                writer.WriteNull();
            }
        }

        private static string PrintElementalType(ElementalType input)
        {
            string returnVal = string.Empty;
            bool first = true;
            int t = 1;
            while (t <= (int)ElementalType.Fairy)
            {
                if (((int)input & t) != 0)
                {
                    if (!first)
                    {
                        returnVal += ",";
                    }

                    returnVal += Enum.GetName(typeof(ElementalType), t);
                    first = false;
                }

                t <<= 1;
            }

            return returnVal;
        }

        private static ElementalType ParseElementalType(string input)
        {
            ElementalType returnVal = 0;
            string[] split = input.Split(',');
            foreach (var str in split)
            {
                ElementalType x;
                if (Enum.TryParse(str, out x))
                {
                    returnVal |= x;
                }
            }

            return returnVal;
        }
    }
}
