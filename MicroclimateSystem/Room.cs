using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroclimateSystem
{
    public class Room
    {
        public double Length { get; set; } 
        public double Width { get; set; } 
        public double Height { get; set; } 
        public int NumberOfPigs { get; set; } 
        public PigType PigType { get; set; } 
        public bool IsWinter { get; set; }   
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public double VentilationRate { get; set; }
        public double CO2Level { get; set; }
        public double AmmoniaLevel { get; set; }
    }
}
