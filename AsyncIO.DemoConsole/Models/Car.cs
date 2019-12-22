using System;
using System.Collections.Generic;
using System.Text;

namespace AsyncIO.DemoConsole.Models
{
    public class Car
    {
        public string Model { get; set; }

        public double HorsePower { get; set; }

        public DateTime Released { get; set; } = DateTime.Now;
    }
}
