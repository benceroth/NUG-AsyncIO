using CsvHelper.Configuration;
using Newtonsoft.Json;
using System;

namespace AsyncIO
{
    public class AsyncIO
    {
        private readonly CsvConfiguration csvConfiguration;
        private readonly JsonConfiguration jsonConfiguration;

        public AsyncIO()
        {
            this.csvConfiguration = new CsvConfiguration();
            this.jsonConfiguration = new JsonConfiguration();

            this.Conversions = new Conversions(this.csvConfiguration, this.jsonConfiguration);

            this.File = new AsyncFile(this.Conversions);
            this.Directory = new AsyncDirectory(this.File);
        }

        public Formatting JsonFormatting
        {
            get => this.jsonConfiguration.Formatting;
            set => this.jsonConfiguration.Formatting = value;
        }

        public JsonSerializerSettings JsonSerializerSettings
        {
            get => this.jsonConfiguration.SerializerSettings;
            set => this.jsonConfiguration.SerializerSettings = value;
        }

        public Configuration CsvConfiguration
        {
            get => this.csvConfiguration.Configuration;
            set => this.csvConfiguration.Configuration = value;
        }

        public AsyncFile File { get; }

        public AsyncDirectory Directory { get; }

        public Conversions Conversions { get; }
    }
}
