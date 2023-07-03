using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMBOX.Revit.Toolkit.Extension.Data.SelectionFilters
{

    internal class DefaultSelectionFilter : ISelectionFilter
    {
        private readonly Func<Element, bool> _predicate;

        public DefaultSelectionFilter(Func<Element, bool> predicate = null)
        {
            _predicate = predicate;
        }

        public bool AllowElement(Element elem)
        {
            if (_predicate != null)
            {
                return _predicate(elem);
            }
            return true;
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return true;
        }
    }
}
