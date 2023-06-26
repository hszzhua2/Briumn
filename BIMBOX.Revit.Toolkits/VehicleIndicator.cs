using System;

namespace BIMBOX.Revit.Toolkit.Extension
{
    public class VehicleIndicator
    {
        private Random random;

        public VehicleIndicator()
        {
            random = new Random();
        }

        public double CalculateVolumetricArea(string city)
        {
            double baseVolumetricArea = 0.0;

            switch (city)
            {
                case "Beijing":
                    baseVolumetricArea = 1.2;
                    break;
                case "Chongqing":
                    baseVolumetricArea = 1.5;
                    break;
                case "Xian":
                    baseVolumetricArea = 1.5;
                    break;
                case "DeZhouShi":
                    baseVolumetricArea = 2.0;
                    break;
                case "HanDanShi":
                    baseVolumetricArea = 1.2;
                    break;
                case "LuoYangShi":
                    baseVolumetricArea = 2.0;
                    break;
                default:
                    Console.WriteLine("City not found.");
                    break;
            }

            double variation = random.NextDouble() - 0.5;
            double volumetricArea = baseVolumetricArea + variation;

            return volumetricArea;
        }

        public static void Main(string[] args)
        {
            string city = "Beijing"; // Enter the desired city here

            VehicleIndicator vehicleIndicator = new VehicleIndicator();
            double volumetricArea = vehicleIndicator.CalculateVolumetricArea(city);
            Console.WriteLine("City: " + city + ", VolumetricArea: " + volumetricArea.ToString("0.0") + " per 100 sq m");
        }
    }
}
