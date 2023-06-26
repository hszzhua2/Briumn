using System.Collections.Generic;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace BIMBOX.Revit.Tuna.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class WallCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDocument = commandData.Application.ActiveUIDocument;
            Document document = uiDocument.Document;
            Selection selection = uiDocument.Selection;
            Reference elementRef = selection.PickObject(ObjectType.Element);

            Element element = document.GetElement(elementRef);

            if (uiDocument.ActiveGraphicalView is View3D view)
            {
                #region FindNearest

                ReferenceIntersector intersector = new ReferenceIntersector(view);

                BoundingBoxXYZ boundingBox = element.get_BoundingBox(view);

                XYZ centerPoint = (boundingBox.Max + boundingBox.Min) / 2;

                ReferenceWithContext referenceWithContext = intersector.FindNearest(centerPoint, XYZ.BasisX);

                if (referenceWithContext != null)
                {
                    Line line = Line.CreateBound(centerPoint, referenceWithContext.GetReference().GlobalPoint);

                    selection.SetElementIds(new List<ElementId> { referenceWithContext.GetReference().ElementId });

                    // Display the Element ID
                    ElementId elementId = referenceWithContext.GetReference().ElementId;
                    TaskDialog.Show("Element ID", $"The selected element's ID is: {elementId}");
                }
                #endregion
            }

            return Result.Succeeded;
        }
    }
}
