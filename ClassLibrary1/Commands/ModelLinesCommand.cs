using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;

namespace BIMBOX.Revit.Tuna.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class ModelLinesCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                // Get the current document and application objects
                UIDocument uidoc = commandData.Application.ActiveUIDocument;
                Document doc = uidoc.Document;

                // Create a selection filter to only select points
                ISelectionFilter selectionFilter = new PointSelectionFilter();

                // Select two points
                IList<Reference> references = uidoc.Selection.PickObjects(ObjectType.Element, selectionFilter, "Select two points");

                if (references.Count != 2)
                {
                    message = "Please select two points";
                    return Result.Failed;
                }

                // Get the two selected points
                XYZ point1 = ((LocationPoint)doc.GetElement(references[0]).Location).Point;
                XYZ point2 = ((LocationPoint)doc.GetElement(references[1]).Location).Point;

                // Create the bounding box for drawing the model lines
                BoundingBoxXYZ boundingBox = new BoundingBoxXYZ();
                boundingBox.Min = new XYZ(Math.Min(point1.X, point2.X), Math.Min(point1.Y, point2.Y), point1.Z);
                boundingBox.Max = new XYZ(Math.Max(point1.X, point2.X), Math.Max(point1.Y, point2.Y), point1.Z);

                // Create model lines
                using (Transaction transaction = new Transaction(doc, "Draw Model Lines"))
                {
                    transaction.Start();

                    // Create start and end points of the model lines
                    XYZ startPoint = new XYZ(boundingBox.Min.X, boundingBox.Min.Y, boundingBox.Min.Z);
                    XYZ endPoint1 = new XYZ(boundingBox.Max.X, boundingBox.Min.Y, boundingBox.Min.Z);
                    XYZ endPoint2 = new XYZ(boundingBox.Max.X, boundingBox.Max.Y, boundingBox.Min.Z);
                    XYZ endPoint3 = new XYZ(boundingBox.Min.X, boundingBox.Max.Y, boundingBox.Min.Z);

                    // Create four model lines
                    Plane plane = Plane.CreateByNormalAndOrigin(doc.ActiveView.ViewDirection, startPoint);
                    SketchPlane sketchPlane = SketchPlane.Create(doc, plane);

                    ModelCurveArray modelCurves = new ModelCurveArray();
                    modelCurves.Append(doc.Create.NewModelCurve(Line.CreateBound(startPoint, endPoint1), sketchPlane));
                    modelCurves.Append(doc.Create.NewModelCurve(Line.CreateBound(endPoint1, endPoint2), sketchPlane));
                    modelCurves.Append(doc.Create.NewModelCurve(Line.CreateBound(endPoint2, endPoint3), sketchPlane));
                    modelCurves.Append(doc.Create.NewModelCurve(Line.CreateBound(endPoint3, startPoint), sketchPlane));

                    transaction.Commit();
                }

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
        }

        // Custom selection filter to only select points
        public class PointSelectionFilter : ISelectionFilter
        {
            public bool AllowElement(Element elem)
            {
                return elem is FamilyInstance familyInstance && familyInstance.Symbol.Family.FamilyCategory.CategoryType == CategoryType.Model;
            }

            public bool AllowReference(Reference reference, XYZ position)
            {
                return true;
            }
        }
    }
}
