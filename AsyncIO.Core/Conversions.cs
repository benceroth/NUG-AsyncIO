// <copyright file="Conversions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AsyncIO.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using CsvHelper;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Bson;

    /// <summary>
    /// Provides conversions back and forth from objects to Json, Bson, Xml, Csv.
    /// </summary>
    public class Conversions
    {
        private readonly CsvConfiguration csvConfiguration;
        private readonly JsonConfiguration jsonConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="Conversions"/> class.
        /// </summary>
        /// <param name="csvConfiguration">Needed CsvHelper configuration.</param>
        /// <param name="jsonConfiguration">Needed Json configuration.</param>
        internal Conversions(CsvConfiguration csvConfiguration, JsonConfiguration jsonConfiguration)
        {
            this.csvConfiguration = csvConfiguration ?? throw new ArgumentNullException(nameof(csvConfiguration));
            this.jsonConfiguration = jsonConfiguration ?? throw new ArgumentNullException(nameof(jsonConfiguration));
        }

        /// <summary>
        /// Deep copies an object with Bson serializer.
        /// </summary>
        /// <typeparam name="T">Item type.</typeparam>
        /// <param name="item">Item to be deep copied.</param>
        /// <returns>Deep copied clone.</returns>
        public T DeepClone<T>(T item)
        {
            return this.FromBson<T>(this.ToBson(item));
        }

        /// <summary>
        /// Serializes an object to Json string.
        /// </summary>
        /// <param name="item">Item to be serialized.</param>
        /// <returns>Serialized string.</returns>
        public string ToJson(object item)
        {
            return JsonConvert.SerializeObject(item, this.jsonConfiguration.Formatting, this.jsonConfiguration.SerializerSettings);
        }

        /// <summary>
        /// Deserializes string from Json to item.
        /// </summary>
        /// <typeparam name="T">Item type.</typeparam>
        /// <param name="data">Json string.</param>
        /// <returns>Deserialized item.</returns>
        public T FromJson<T>(string data)
        {
            return JsonConvert.DeserializeObject<T>(data, this.jsonConfiguration.SerializerSettings);
        }

        /// <summary>
        /// Serializes an object to Bson bytes.
        /// </summary>
        /// <param name="item">Item to be serialized.</param>
        /// <returns>Serialized bytes.</returns>
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

        /// <summary>
        /// Deserializes bytes from Bson to item.
        /// </summary>
        /// <typeparam name="T">Item type.</typeparam>
        /// <param name="data">Bson bytes.</param>
        /// <returns>Deserialized item.</returns>
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

        /// <summary>
        /// Serializes an object to Xml string.
        /// </summary>
        /// <param name="item">Item to be serialized.</param>
        /// <returns>Serialized string.</returns>
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

        /// <summary>
        /// Deserializes string from Xml to item.
        /// </summary>
        /// <typeparam name="T">Item type.</typeparam>
        /// <param name="data">Xml string.</param>
        /// <returns>Deserialized item.</returns>
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

        /// <summary>
        /// Serializes items to Csv string.
        /// </summary>
        /// <typeparam name="T">Items type.</typeparam>
        /// <param name="items">Items to be serialized.</param>
        /// <returns>Serialized string.</returns>
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

        /// <summary>
        /// Deserializes string from Csv to items.
        /// </summary>
        /// <typeparam name="T">Items type.</typeparam>
        /// <param name="data">Csv string.</param>
        /// <returns>Deserialized items.</returns>
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
