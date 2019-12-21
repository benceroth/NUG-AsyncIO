using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace AsyncIO
{
    internal class CsvConfiguration
    {
        internal CsvConfiguration()
        {
            this.Configuration = new Configuration();
        }

        internal Configuration Configuration { get; set; }
    }
}
