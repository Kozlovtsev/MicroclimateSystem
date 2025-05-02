using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroclimateSystem
{
    public class CalculationResult
    {
        public string RoomName { get; set; }
        public DateTime Date { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public double VentilationRate { get; set; }
        public string Notes { get; set; }
    }

}
