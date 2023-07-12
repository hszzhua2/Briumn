using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMBOX.Revit.Tuna.Helpers.UserControls.SelectFileReferences.Model
{
    public class SelectFileReferencesModel
    {
        public ExternalFileReferenceType ReferenceType { get; set; }
        public bool IsSelected { get; set; }
        public string Text
        {
            get
            {
                return ReferenceType.ToString();
            }
        }

        public SelectFileReferencesModel(ExternalFileReferenceType externalFileReferenceType)
        {
            ReferenceType = externalFileReferenceType;
        }
    }
}
