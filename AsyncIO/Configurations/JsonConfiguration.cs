// <copyright file="JsonConfiguration.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AsyncIO
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Newtonsoft.Json;

    /// <summary>
    /// Provides configuration for Newtonsoft.Json.
    /// </summary>
    internal class JsonConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonConfiguration"/> class.
        /// </summary>
        internal JsonConfiguration()
        {
            this.Formatting = Formatting.Indented;

            this.SerializerSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                PreserveReferencesHandling = PreserveReferencesHandling.All,
            };
        }

        /// <summary>
        /// Gets or sets Json formatting.
        /// </summary>
        internal Formatting Formatting { get; set; }

        /// <summary>
        /// Gets or sets Json serializer settings.
        /// </summary>
        internal JsonSerializerSettings SerializerSettings { get; set; }
    }
}
