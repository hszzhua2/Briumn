using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Analysis;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BIMBOX.Revit.Tuna.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class Pt2RoomPath : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            try
            {
                // Get all room elements
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                ICollection<Element> rooms = collector.OfCategory(BuiltInCategory.OST_Rooms).OfClass(typeof(SpatialElement)).ToElements();

                // Create array to store all room location points
                List<XYZ> roomPoints = new List<XYZ>();

                // Iterate through each room element and get its location point
                foreach (Element room in rooms)
                {
                    SpatialElement spatialElement = room as SpatialElement;
                    LocationPoint locationPoint = spatialElement.Location as LocationPoint;

                    // Check if locationPoint is not null before accessing its properties
                    if (locationPoint != null)
                    {
                        XYZ roomLocation = locationPoint.Point;
                        roomPoints.Add(roomLocation);
                    }
                }

                // Flatten the list of room location points
                List<XYZ> flattenedRoomPoints = roomPoints.SelectMany(x => x is IList ? ((IList)x).Cast<XYZ>() : new List<XYZ> { x }).ToList();

                // Create path of travel
                using (Transaction t = new Transaction(doc, "Create Path of Travel"))
                {
                    t.Start();

                    // Use the first room location point as the starting point
                    XYZ firstPoint = uidoc.Selection.PickPoint("Select first point for Path of Travel");

                    // Check if any rooms were found
                    if (flattenedRoomPoints.Count == 0)
                    {
                        TaskDialog.Show("No Rooms Found", "No rooms were found in the model. Please add rooms or check the model.");
                        return Result.Failed;
                    }

                    // Iterate through all room location points and connect them in order
                    foreach (XYZ endPoint in flattenedRoomPoints)
                    {
                        PathOfTravel.Create(doc.ActiveView, firstPoint, endPoint);
                        //firstPoint = endPoint;
                    }

                    t.Commit();
                }

                return Result.Succeeded;
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                return Result.Cancelled;
            }
        }
    }
}