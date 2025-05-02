using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroclimateSystem
{
    public static class ClimateCalculator
    {
        public static double CalculateOptimalTemperature(PigType pigType, bool isWinter)
        {
            double baseTemperature = pigType switch
            {
                PigType.Piglet => 30,
                PigType.Growing => 22,
                PigType.Adult => 18,
                _ => 20
            };

            return isWinter ? baseTemperature + 2 : baseTemperature - 2;
        }

        public static double CalculateOptimalHumidity(bool isWinter)
        {
            return isWinter ? 70 : 60;
        }

        public static double CalculateVentilationRate(int numberOfPigs, PigType pigType, bool isWinter)
        {
            double weightPerPig = pigType switch
            {
                PigType.Piglet => 10,
                PigType.Growing => 50,
                PigType.Adult => 100,
                _ => 50
            };

            double baseVentilation = numberOfPigs * weightPerPig * 25;

            return isWinter ? baseVentilation * 0.8 : baseVentilation * 1.2;
        }

        public static double CalculateCO2Level(int numberOfPigs, PigType pigType)
        {
            return 500 + (numberOfPigs * 10);
        }

        public static double CalculateAmmoniaLevel(int numberOfPigs)
        {
            return 5 + (numberOfPigs * 1);
        }
    }
}
