using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB.Analysis;
using System.Net;

namespace BIMBOX.Revit.Tuna.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class Points : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Get the current document and application
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            // Prompt the user to select two elements
            List<Reference> pickedReferences = uidoc.Selection.PickObjects(ObjectType.Element, "Select two elements").ToList();
            if (pickedReferences == null || pickedReferences.Count != 2)
                return Result.Cancelled;

            // Get the XYZ coordinates of the selected elements
            List<XYZ> points = new List<XYZ>();

            foreach (Reference reference in pickedReferences)
            {
                Element element = doc.GetElement(reference);

                // Assuming the selected elements have LocationPoint as their location
                if (element.Location is LocationPoint locationPoint)
                {
                    XYZ point = locationPoint.Point;
                    points.Add(point);
                }
            }

            if (points.Count != 2)
                return Result.Cancelled;

            FilteredElementCollector collector = new FilteredElementCollector(doc);
            ICollection<Element> rooms = collector.OfCategory(BuiltInCategory.OST_Rooms)
                .OfClass(typeof(SpatialElement))
                .ToElements();

            List<XYZ> roomPoints = new List<XYZ>();

            foreach (Element room in rooms)
            {
                SpatialElement spatialElement = room as SpatialElement;
                LocationPoint locationPoint = spatialElement.Location as LocationPoint;

                if (locationPoint != null)
                {
                    XYZ roomLocation = locationPoint.Point;
                    roomPoints.Add(roomLocation);
                }
            }

            using (Transaction t = new Transaction(doc, "Create Path of Travel"))
            {
                t.Start();

                foreach (XYZ startPoint in roomPoints)
                {
                    PathOfTravel path1 = PathOfTravel.Create(doc.ActiveView, startPoint, points[0]);
                    PathOfTravel path2 = PathOfTravel.Create(doc.ActiveView, startPoint, points[1]);
                    // Get the curve of the path

                    double tp_length = path1.get_Parameter(Autodesk.Revit.DB.BuiltInParameter.CURVE_ELEM_LENGTH).AsDouble();
                    double tp_length1 = path2.get_Parameter(Autodesk.Revit.DB.BuiltInParameter.CURVE_ELEM_LENGTH).AsDouble();
                    // Calculate the length of each path curve

                    List<double> pathLengths = new List<double> { tp_length, tp_length1 };

                    // Print the lengths of the path curves
                    foreach (double pathLength in pathLengths)
                    {
                        TaskDialog.Show("Path Length", "Length: " + pathLength.ToString());
                    }
                }
                t.Commit();
            }
            return Result.Succeeded;
        }
    }
}