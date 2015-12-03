using System;

namespace Gu.Units.Json
{
    using System.Globalization;
    using Newtonsoft.Json;

    public class AngleConverter : JsonConverter
    {
        public static readonly AngleConverter Default = new AngleConverter(AngleUnit.Radians);
        public static readonly AngleConverter Radians = new AngleConverter(AngleUnit.Radians);
        public static readonly AngleConverter Degrees = new AngleConverter(AngleUnit.Degrees);
        private readonly AngleUnit unit;

        private AngleConverter(AngleUnit unit)
        {
            this.unit = unit;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var angle = (Angle)value;
            serializer.Serialize(writer, angle.ToString(this.unit, writer.Culture));
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Angle);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var stringValue = reader.Value as string;
            return Angle.Parse(stringValue, reader.Culture);
        }
    }
}
