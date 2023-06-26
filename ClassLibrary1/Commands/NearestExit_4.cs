using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Analysis;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BIMBOX.Revit.Tuna.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class NearestExit_4 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Step 1: Get the current document and application
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            try
            {
                // Step 2: Let the user select PathOfTravel instances
                List<Reference> pathgroup = new List<Reference>();
                List<Reference> pickedReferences = uidoc.Selection.PickObjects(ObjectType.Element, "Select PathOfTravel instances").ToList();
                // Step 3: Calculate and collect the lengths of each selected path in millimeters, along with their ElementId
                List<Tuple<double, ElementId>> pathLengths = new List<Tuple<double, ElementId>>();

                foreach (Reference reference in pickedReferences)
                {
                    Element pathElement = doc.GetElement(reference.ElementId);

                    if (pathElement is PathOfTravel path)
                    {
                        double pathLength = 0.0;

                        foreach (Curve curve in path.GetCurves())
                        {
                            pathLength += curve.Length * 0.3048; // Convert from feet to meter
                        }

                        pathLengths.Add(new Tuple<double, ElementId>(pathLength, path.Id));
                    }
                }

                // Step 4: Sort the path lengths in ascending order
                pathLengths.Sort((x, y) => x.Item1.CompareTo(y.Item1));

                // Step 5: Keep the shortest path and remove the rest
                using (Transaction transaction = new Transaction(doc, "Keep Shortest Path"))
                {
                    transaction.Start();

                    foreach (var pathLength in pathLengths.Skip(1)) // Skip the shortest path
                    {
                        ElementId pathId = pathLength.Item2;
                        doc.Delete(pathId);
                    }

                    transaction.Commit();
                }

                // Step 6: Display the lengths and associated ElementId in a dialog
                StringBuilder dialogMessage = new StringBuilder();
                dialogMessage.AppendLine("路径从小到大为 (Sorted):");

                foreach (var pathLength in pathLengths)
                {
                    string lengthInMeters = pathLength.Item1.ToString("F2") + " m";
                    string elementId = pathLength.Item2.ToString();

                    dialogMessage.AppendLine("ElementId: " + elementId + ", Length: " + lengthInMeters);
                }

                TaskDialog.Show("路径长度", dialogMessage.ToString());

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
