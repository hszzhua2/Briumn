using BIMBOX.Revit.Toolkit.Extension.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMBOX.Revit.Toolkit.Extension.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class AvailabilityAttribute : Attribute
    {
        public AvailabilityAttribute(AvailabilityMode availability)
        {
            Availability = availability;
        }

        public AvailabilityMode Availability { get; }
    }
}
