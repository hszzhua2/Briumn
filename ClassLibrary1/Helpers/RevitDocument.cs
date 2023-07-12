using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMBOX.Revit.Tuna.Helpers
{
    internal class RevitDocument
    {
        public static bool IsDocumentNotProjectDoc(Document document)
        {
            if (document.IsFamilyDocument)
            {
                string warningMessage = "The active document is a Family document, \n" +
                    "please use the tool in a Project document.";
                MessageWindows.AlertMessage("Warning", warningMessage);
                return true;
            }
            return false;
        }
    }
}
