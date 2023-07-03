﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMBOX.Revit.Toolkit.Extension.Data
{/// <summary>
 /// Invalid
 /// </summary>
    [Flags]
    public enum AvailabilityMode
    {
        /// <summary>
        /// 
        /// </summary>
        Always = 0,

        /// <summary>
        /// 
        /// </summary>
        OnlyDocument = 1,

        /// <summary>
        /// 
        /// </summary>
        OnlyFamily = 2,

        /// <summary>
        /// 
        /// </summary>
        OnlyProject = 4,

        /// <summary>
        /// 
        /// </summary>
        OnlyThreeDView = 8,

        /// <summary>
        /// 
        /// </summary>
        OnlyPlanView = 16,
    }
}
