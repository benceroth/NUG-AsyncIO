using System;
using System.Collections.Generic;
using System.Text;

namespace AsyncIO.DemoConsole.Models
{
    public class Brand
    {
        public string Name { get; set; }

        public string Owner { get; set; }

        public List<Car> Cars { get; set; } = new List<Car>();
    }
}
