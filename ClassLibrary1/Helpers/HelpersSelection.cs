﻿using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMBOX.Revit.Tuna.Helpers
{
    class HelpersSelection
    {
        /// <summary>
        /// Function for user to pick up line
        /// </summary>
        /// <param name="uidoc"></param>
        /// <returns></returns>
        public static ElementId PickLine(UIDocument uidoc)
        {
            // Selection filter
            ISelectionFilter selFilter = new LineSelectionFilter();

            try
            {
                // Prompt user to select curve that intersects with rooms
                ElementId curveId = uidoc.Selection.PickObject(ObjectType.Element, selFilter).ElementId;
                return curveId;
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException e)
            {
                TaskDialog.Show("Operation Canceled", e.Message);
                return null;
            }
        }

        /// <summary>
        /// Class filter to pick up only lines
        /// </summary>
        public class LineSelectionFilter : ISelectionFilter
        {
            public bool AllowElement(Element element)
            {
                if (element.Category != null && element.Category.Name == "Lines")
                {
                    return true;
                }
                return false;
            }
            // Overrride AllowReference method
            public bool AllowReference(Reference refer, XYZ point)
            {
                return false;
            }
        }

        /// <summary>
        /// Pick eleemnts in model and store them in order
        /// </summary>
        /// <param name="uidoc"></param>
        /// <returns></returns>
        public static IList<ElementId> PickOrderedElements(UIDocument uidoc)
        {
            IList<ElementId> elementIds = new List<ElementId>();
            bool flag = true;

            while (flag)
            {
                try
                {
                    Reference reference = uidoc.Selection.PickObject(ObjectType.Element);
                    elementIds.Add(reference.ElementId);
                }
                catch
                {
                    flag = false;
                }
            }

            return elementIds;
        }

        /// <summary>
        /// Create group from element ids
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="elementIds"></param>
        /// <returns></returns>
        public static Group CreateGroupFromElementIds(Document doc, ICollection<ElementId> elementIds)
        {
            Group group = null;

            if (elementIds.Count > 0)
            {
                if (doc.IsFamilyDocument)
                    group = doc.FamilyCreate.NewGroup(elementIds);
                else
                    group = doc.Create.NewGroup(elementIds);
            }

            return group;
        }
    }
}
