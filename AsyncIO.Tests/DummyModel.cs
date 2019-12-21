using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace AsyncIO.Tests
{
    [XmlRoot("DummyModel")]
    public class DummyModel
    {
        public string Name { get; set; } = "Dummy";
    }
}
