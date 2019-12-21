// <copyright file="CsvConfiguration.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AsyncIO
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using CsvHelper.Configuration;

    /// <summary>
    /// Provides configuration for CsvHelper.
    /// </summary>
    internal class CsvConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CsvConfiguration"/> class.
        /// </summary>
        internal CsvConfiguration()
        {
            this.Configuration = new Configuration();
        }

        /// <summary>
        /// Gets or sets CsvHelper configuration.
        /// </summary>
        internal Configuration Configuration { get; set; }
    }
}
