using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Analysis;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BIMBOX.Revit.Tuna.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class Combination : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Step 1: Get the current document and application
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            try
            {
                // Step 2: Prompt the user to select objects and store their exit points
                List<XYZ> exitPoints = new List<XYZ>();
                List<Reference> pickedReferences = uidoc.Selection.PickObjects(ObjectType.Element, "Select objects").ToList();

                foreach (Reference reference in pickedReferences)
                {
                    Element element = doc.GetElement(reference);

                    // Assuming the selected elements have LocationPoint as their location
                    if (element.Location is LocationPoint locationPoint)
                    {
                        XYZ exitPoint = locationPoint.Point;
                        exitPoints.Add(exitPoint);
                    }
                }

                // Step 3: Retrieve door locations
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                ICollection<Element> doors = collector.OfCategory(BuiltInCategory.OST_Doors)
                    .OfClass(typeof(FamilyInstance))
                    .ToElements();

                List<XYZ> doorPoints = new List<XYZ>();

                foreach (Element door in doors)
                {
                    FamilyInstance familyInstance = door as FamilyInstance;
                    LocationPoint locationPoint = familyInstance.Location as LocationPoint;

                    if (locationPoint != null)
                    {
                        XYZ doorLocation = locationPoint.Point;
                        doorPoints.Add(doorLocation);
                    }
                }

                // Step 4: Check for duplicate points and remove them from doorPoints
                List<XYZ> uniqueDoorPoints = doorPoints.Distinct().ToList();

                // Step 5: Create PathOfTravel instances and find shortest path for each exit point
                using (Transaction transaction = new Transaction(doc, "Create Path of Travel"))
                {
                    transaction.Start();

                    Dictionary<XYZ, List<ElementId>> pathStartGroup = new Dictionary<XYZ, List<ElementId>>();

                    foreach (XYZ exitPoint in exitPoints)
                    {
                        double shortestPathLength = double.MaxValue;
                        PathOfTravel shortestPath = null;

                        foreach (XYZ doorPoint in uniqueDoorPoints)
                        {
                            // Check if exitPoint and doorPoint are the same
                            if (exitPoint.IsAlmostEqualTo(doorPoint))
                                continue;

                            PathOfTravel path = PathOfTravel.Create(doc.ActiveView, doorPoint, exitPoint);

                            // Calculate path length and update shortest path if necessary
                            double pathLength = 0.0;
                            foreach (Curve curve in path.GetCurves())
                            {
                                pathLength += curve.Length;
                            }

                            if (pathLength < shortestPathLength)
                            {
                                shortestPathLength = pathLength;
                                shortestPath = path;
                            }
                        }

                        if (shortestPath != null)
                        {
                            // Group PathOfTravel instances by path start
                            XYZ pathStart = shortestPath.PathStart;
                            if (!pathStartGroup.ContainsKey(pathStart))
                            {
                                pathStartGroup[pathStart] = new List<ElementId>();
                            }
                            pathStartGroup[pathStart].Add(shortestPath.Id);

                            // Do something with the shortest path (e.g., store, display, etc.)

                            // Delete non-shortest paths in the group
                            List<ElementId> nonShortestPaths = pathStartGroup[pathStart].Where(id => id != shortestPath.Id).ToList();
                            foreach (ElementId pathId in nonShortestPaths)
                            {
                                Element path = doc.GetElement(pathId);
                                doc.Delete(path.Id);
                            }
                        }
                    }
                    transaction.Commit();
                }
                return Result.Succeeded;
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                // Catch and handle the OperationCanceledException here
                return Result.Cancelled;
            }
        }
    }
}
