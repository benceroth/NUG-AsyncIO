// <copyright file="IO.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AsyncIO.Core
{
    using System;
    using CsvHelper.Configuration;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    /// <summary>
    /// Provides IO handling features.
    /// </summary>
    public class IO
    {
        private readonly Transaction transaction;
        private readonly CsvConfiguration csvConfiguration;
        private readonly JsonConfiguration jsonConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="IO"/> class.
        /// </summary>
        /// <param name="logger">Injected logger.</param>
        public IO(ILogger logger = null)
        {
            this.csvConfiguration = new CsvConfiguration();
            this.jsonConfiguration = new JsonConfiguration();

            this.transaction = new Transaction(logger);
            this.Conversions = new Conversions(this.csvConfiguration, this.jsonConfiguration);

            this.File = new AsyncFile(this.Conversions, this.transaction);
            this.Directory = new AsyncDirectory(this.File);
        }

        /// <summary>
        /// Gets or sets Json formatting.
        /// </summary>
        public Formatting JsonFormatting
        {
            get => this.jsonConfiguration.Formatting;
            set => this.jsonConfiguration.Formatting = value;
        }

        /// <summary>
        /// Gets or sets Json serializer settings.
        /// </summary>
        public JsonSerializerSettings JsonSerializerSettings
        {
            get => this.jsonConfiguration.SerializerSettings;
            set => this.jsonConfiguration.SerializerSettings = value;
        }

        /// <summary>
        /// Gets or sets Csv configuration.
        /// </summary>
        public Configuration CsvConfiguration
        {
            get => this.csvConfiguration.Configuration;
            set => this.csvConfiguration.Configuration = value;
        }

        /// <summary>
        /// Gets file handling features.
        /// </summary>
        public AsyncFile File { get; }

        /// <summary>
        /// Gets directory handling features.
        /// </summary>
        public AsyncDirectory Directory { get; }

        /// <summary>
        /// Gets conversions back and forth from objects to Json, Bson, Xml, Csv.
        /// </summary>
        public Conversions Conversions { get; }

        /// <summary>
        /// Begins an IO transaction.
        /// </summary>
        public void BeginTransaction()
        {
            this.transaction.BeginTransaction();
        }

        /// <summary>
        /// Commits all changes.
        /// </summary>
        public void Commit()
        {
            this.transaction.Commit();
        }

        /// <summary>
        /// Rollbacks all changes.
        /// </summary>
        public void Rollback()
        {
            this.transaction.Rollback();
        }
    }
}
