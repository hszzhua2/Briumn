using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Analysis;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;
namespace BIMBOX.Revit.Tuna.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class FireProofArea : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Step 1: Get the current document and application
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            // Step 2: Prompt the user to select elements
            List<Reference> pickedReferences = uidoc.Selection.PickObjects(ObjectType.Element, "Select two elements").ToList();

            // Step 3: Get the XYZ coordinates of the selected elements
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

            // Step 4: Retrieve room locations
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
            // Step 5: Create PathOfTravel instances
            XYZ shortestPathStart = null;
            XYZ shortestPathEnd = null;
            double shortestPathLength = double.MaxValue;

            using (Transaction transaction = new Transaction(doc, "Create Path of Travel"))
            {
                transaction.Start();

                foreach (XYZ roomLocation in roomPoints)
                {
                    foreach (XYZ point in points)
                    {
                        PathOfTravel path = PathOfTravel.Create(doc.ActiveView, point, roomLocation);

                        // Check if the path and curves are valid
                        if (path != null && path.GetCurves() != null)
                        {
                            double pathLength = path.GetCurves().Sum(c => c.Length);
                            if (pathLength < shortestPathLength)
                            {
                                shortestPathLength = pathLength;
                                shortestPathStart = path.PathStart;
                                shortestPathEnd = path.PathEnd;
                            }
                            else
                            {
                                // Delete the non-shortest paths
                                doc.Delete(path.Id);
                            }
                        }
                    }
                }

                transaction.Commit();
            }



            // Additional logic if needed...

            return Result.Succeeded;
        }
    }
}