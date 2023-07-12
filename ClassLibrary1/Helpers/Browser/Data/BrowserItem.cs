using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMBOX.Revit.Tuna.Helpers.Browser.Data
{
    /// <summary>
    /// Information about a directory item; drives, folders, and files
    /// </summary>
    public class BrowserItem
    {
        /// <summary>
        /// The type of this item
        /// </summary>
        public BrowserItemType Type { get; set; }

        /// <summary>
        /// The absolute path to this item
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        /// Name of this directory item
        /// </summary>
        public string Name { get { return Type == BrowserItemType.Drive ? FullPath : BrowserStructure.GetFileFolderName(FullPath); } }
    }
}
