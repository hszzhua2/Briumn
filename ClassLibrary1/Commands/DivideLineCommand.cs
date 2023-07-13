/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMBOX.Revit.Tuna.Commands
{
    using Autodesk.Revit.Attributes;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;
    using Autodesk.Revit.UI.Selection;
    using System.Collections.Generic;

    *//*namespace BIMBOX.Revit.Tuna.Commands
    {
        [Transaction(TransactionMode.Manual)]
        public class DivideLineCommand : IExternalCommand
        {
            public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
            {
                // Get the Revit document
                UIDocument uidoc = commandData.Application.ActiveUIDocument;
                Document doc = uidoc.Document;

                // Retrieve the input parameters from the user interface
                List<List<Curve>> curveLists = GetInputCurves(uidoc);

                double distance = GetInputDistance(uidoc);
                bool addLast = GetInputAddLast(uidoc);
                bool displayTextDots = GetInputDisplayTextDots(uidoc);
                string dotPrefix = GetInputDotPrefix(uidoc);
                int dotHeight = GetInputDotHeight(uidoc);

                // Process the input curves and create regions
                List<List<ElementId>> regionIds = CreateRegions(doc, curveLists, distance, addLast);

                // Display text dots if required
                if (displayTextDots)
                {
                    DisplayTextDots(doc, regionIds, dotPrefix, dotHeight);
                }

                return Result.Succeeded;
            }

            private List<List<ElementId>> CreateRegions(Document doc, List<List<Curve>> curveLists, double distance, bool addLast)
            {
                throw new NotImplementedException();
            }

            private int GetInputDotHeight(UIDocument uidoc)
            {
                throw new NotImplementedException();
            }

            private string GetInputDotPrefix(UIDocument uidoc)
            {
                throw new NotImplementedException();
            }

            private bool GetInputDisplayTextDots(UIDocument uidoc)
            {
                throw new NotImplementedException();
            }

            private bool GetInputAddLast(UIDocument uidoc)
            {
                throw new NotImplementedException();
            }

            private double GetInputDistance(UIDocument uidoc)
            {
                throw new NotImplementedException();
            }

            private List<List<Curve>> GetInputCurves(UIDocument uidoc)
            {
                throw new NotImplementedException();
            }

            private void DisplayTextDots(Document doc, List<List<ElementId>> regionIds, string dotPrefix, int dotHeight)
            {
                // Display text dots based on the created regions
                // Replace this with your own implementation to display text dots
            }
        }
    }
*//*



    [Transaction(TransactionMode.Manual)]
    public class CreatePerpendicularWallCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Get the Revit application and document
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Document doc = uiDoc.Document;

            try
            {
                // Pick a wall using the Revit selection
                Reference pickedWallRef = uiDoc.Selection.PickObject(ObjectType.Element, "Select a wall");
                Element pickedWall = doc.GetElement(pickedWallRef.ElementId) as Wall;

                if (pickedWall != null)
                {
                    // Get the start and end points of the wall
                    LocationCurve wallLocation = pickedWall.Location as LocationCurve;
                    XYZ startPoint = wallLocation.Curve.GetEndPoint(0);
                    XYZ endPoint = wallLocation.Curve.GetEndPoint(1);

                    // Get the wall type
                    ElementId wallTypeId = pickedWall.WallType.Id;
                    WallType wallType = doc.GetElement(wallTypeId) as WallType;

                    // Get the point on the wall at parameter 3.6 meters
                    double parameter = 3.6;
                    XYZ pointOnWall = wallLocation.Curve.Evaluate(parameter, true);

                    // Calculate the direction perpendicular to the wall
                    XYZ wallDirection = (endPoint - startPoint).Normalize();
                    XYZ perpendicularDirection = new XYZ(-wallDirection.Y, wallDirection.X, 0);

                    // Create a new wall perpendicular to the old wall
                    using (Transaction trans = new Transaction(doc, "Create Perpendicular Wall"))
                    {
                        trans.Start();

                        XYZ newWallStart = pointOnWall - perpendicularDirection * 5; // Assuming 10 meters length, divided by 2
                        XYZ newWallEnd = pointOnWall + perpendicularDirection * 5;

                        Line newWallLine = Line.CreateBound(newWallStart, newWallEnd);
                        Wall newWall = Wall.Create(doc, newWallLine, wallTypeId, pickedWall.LevelId, pickedWall.Height, 0, false, false);

                        trans.Commit();
                    }

                    return Result.Succeeded;
                }
                else
                {
                    message = "No wall element was picked.";
                    return Result.Failed;
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
        }
    }
}
*/
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace BIMBOX.Revit.Tuna.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class PickWallAndCreateNewWall : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            try
            {
                // Pick a wall
                Reference pickedWallRef = uidoc.Selection.PickObject(ObjectType.Element, new WallSelectionFilter(), "Select a wall");
                Wall pickedWall = doc.GetElement(pickedWallRef) as Wall;

                // Get start and end points of the picked wall
                LocationCurve wallLocation = pickedWall.Location as LocationCurve;
                XYZ startPoint = wallLocation.Curve.GetEndPoint(0);
                XYZ endPoint = wallLocation.Curve.GetEndPoint(1);

                // Get the wall type of the picked wall
                WallType pickedWallType = doc.GetElement(pickedWall.GetTypeId()) as WallType;

                // Get the point on the wall at parameter 3.6 meters
                XYZ pointOnWall = wallLocation.Curve.Evaluate(3600, true);

                // Create a new line perpendicular to the old wall with a length of 10 meters
                XYZ direction = (endPoint - startPoint).Normalize();
                XYZ perpendicularDirection = new XYZ(-direction.Y, direction.X, 0);
                Line newWallLine = Line.CreateBound(pointOnWall, pointOnWall + perpendicularDirection * 1000);

                // Create a new wall using the same wall type as the picked wall
                using (Transaction t = new Transaction(doc, "Create New Wall"))
                {
                    t.Start();
                    Wall.Create(doc, newWallLine, pickedWallType.Id, pickedWall.LevelId, 10, 0, false, false);
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

    public class WallSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            return elem is Wall;
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return false;
        }
    }
}
