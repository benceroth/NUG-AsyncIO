using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AsyncIO
{
    internal class JsonConfiguration
    {
        internal JsonConfiguration()
        {
            this.Formatting = Formatting.Indented;

            this.SerializerSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                PreserveReferencesHandling = PreserveReferencesHandling.All,
            };
        }

        internal Formatting Formatting { get; set; }

        internal JsonSerializerSettings SerializerSettings { get; set; }
    }
}
