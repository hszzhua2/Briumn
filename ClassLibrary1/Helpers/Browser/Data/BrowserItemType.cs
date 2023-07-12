using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMBOX.Revit.Tuna.Helpers.Browser.Data
{
    /// <summary>
    /// The type of a directory item
    /// </summary>
    public enum BrowserItemType
    {
        /// <summary>
        /// A logical drive
        /// </summary>
        Drive,
        /// <summary>
        /// A physical file
        /// </summary>
        File,
        /// <summary>
        /// A folder
        /// </summary>
        Folder,
        /// <summary>
        /// CAD drawings
        /// </summary>
        Dwg,
        /// <summary>
        /// Pdf files
        /// </summary>
        Pdf,
        /// <summary>
        /// Image Files
        /// </summary>
        Image,
        /// <summary>
        /// Text files
        /// </summary>
        Txt,
        /// <summary>
        /// Excel files
        /// </summary>
        Xls,
        /// <summary>
        /// Revit projects, families, templates files
        /// </summary>
        Revit
    }
}

