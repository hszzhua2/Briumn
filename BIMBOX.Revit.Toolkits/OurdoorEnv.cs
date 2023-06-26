using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMBOX.Revit.Toolkit.Extension
{
    public class FloorHeightIndex
    {
        public static string GetFloorHeightDescription(int floorNumber)
        {
            switch (floorNumber)
            {
                case 1:
                    return "First Floor: 4.8m to 5.7m, 5.1m to 5.4m is better";
                case 2:
                case 3:
                case 4:
                case 5:
                    return "Floor " + floorNumber + ": 4.3m to 5.1m, 4.5m is better";
                default:
                    return "Invalid floor number";
            }
        }

        public static string GetImpatienceRoomHeightDescription()
        {
            return "Impatience Room: 4.2m is better, 3.9m to 4.5m, higher than 3.6m";
        }
    }

}
