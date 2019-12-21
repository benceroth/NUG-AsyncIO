using CsvHelper;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace AsyncIO
{
    public class Conversions
    {
        private readonly CsvConfiguration csvConfiguration;
        private readonly JsonConfiguration jsonConfiguration;

        internal Conversions(CsvConfiguration csvConfiguration, JsonConfiguration jsonConfiguration)
        {
            this.csvConfiguration = csvConfiguration ?? throw new ArgumentNullException(nameof(csvConfiguration));
            this.jsonConfiguration = jsonConfiguration ?? throw new ArgumentNullException(nameof(jsonConfiguration));
        }

        public T DeepClone<T>(T item)
        {
            return this.FromBson<T>(this.ToBson(item));
        }

        public string ToJson(object item)
        {
            return JsonConvert.SerializeObject(item, this.jsonConfiguration.Formatting, this.jsonConfiguration.SerializerSettings);
        }

        public T FromJson<T>(string data)
        {
            return JsonConvert.DeserializeObject<T>(data, this.jsonConfiguration.SerializerSettings);
        }

        public byte[] ToBson(object item)
        {
            using (var ms = new MemoryStream())
            {
                using (var writer = new BsonDataWriter(ms))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(writer, item);
                }
                return ms.ToArray();
            }
        }

        public T FromBson<T>(byte[] data)
        {
            using (var ms = new MemoryStream(data))
            {
                using (var reader = new BsonDataReader(ms))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    return serializer.Deserialize<T>(reader);
                }
            }
        }

        public string ToXml(object item)
        {
            var serializer = new XmlSerializer(item.GetType());
            using (StringWriter writer = new StringWriter())
            {
                using (var xml = XmlWriter.Create(writer))
                {
                    serializer.Serialize(xml, item);
                    return writer.ToString();
                }
            }
        }

        public T FromXml<T>(string data)
            where T : class
        {
            var serializer = new XmlSerializer(typeof(T));
            using (StringReader reader = new StringReader(data))
            {
                using (var xml = XmlReader.Create(reader))
                {
                    return serializer.Deserialize(xml) as T;
                }
            }
        }

        public string ToCsv<T>(IEnumerable<T> items)
        {
            using (TextWriter writer = new StringWriter())
            {
                using (var csv = new CsvWriter(writer, this.csvConfiguration.Configuration))
                {
                    csv.WriteRecords(items);
                    writer.Flush();
                    return writer.ToString();
                }
            }
        }

        public IEnumerable<T> FromCsv<T>(string data)
        {
            using (TextReader reader = new StringReader(data))
            {
                using (var csv = new CsvReader(reader, this.csvConfiguration.Configuration))
                {
                    while (csv.Read())
                    {
                        yield return csv.GetRecord<T>();
                    }
                }
            }
        }
    }
}
