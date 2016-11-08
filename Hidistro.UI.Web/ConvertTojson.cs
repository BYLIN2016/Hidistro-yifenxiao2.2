namespace Hidistro.UI.Web
{
    using Newtonsoft.Json;
    using System;
    using System.Data;

    public class ConvertTojson : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(DataTable).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value)
        {
            DataTable table = (DataTable) value;
            writer.WriteStartArray();
            foreach (DataRow row in table.Rows)
            {
                writer.WriteStartObject();
                foreach (DataColumn column in table.Columns)
                {
                    writer.WritePropertyName(column.ColumnName);
                    writer.WriteValue(row[column].ToString());
                }
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
        }
    }
}

