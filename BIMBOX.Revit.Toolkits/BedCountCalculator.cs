using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMBOX.Revit.Toolkit.Extension
{
    public class BedCountCalculator
    {
        //床位数扩展
        public static int calculateSquarePerBed(int bedCount)
        {
            int squarePerBed = 0;

            if (bedCount < 200)
            {
                squarePerBed = 110;
            }
            else if (bedCount >= 200 && bedCount < 500)
            {
                squarePerBed = 113;
            }
            else if (bedCount >= 500 && bedCount < 800)
            {
                squarePerBed = 116;
            }
            else if (bedCount >= 800 && bedCount < 1200)
            {
                squarePerBed = 114;
            }
            else if (bedCount >= 1200)
            {
                squarePerBed = 112;
            }

            return squarePerBed;
        }

        public static void main(String[] args)
        {
            int bedCount = 1000; // Enter the desired bed count here

            int squarePerBed = calculateSquarePerBed(bedCount);
            

        }
    }

}
