using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

    [Transaction(TransactionMode.Manual)]
    public class MultiSelectAndGetBoundingBox_Good : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            // Prompt the user to select elements
            IList<Reference> selectedRefs = uiDoc.Selection.PickObjects(ObjectType.Element);

            // Loop through the selected elements and get their bounding boxes
            foreach (Reference selectedRef in selectedRefs)
            {
                Element selectedElem = doc.GetElement(selectedRef);

                // Get the bounding box of the selected element
                BoundingBoxXYZ bbox = selectedElem.get_BoundingBox(null);

                if (bbox == null) // check if bounding box is null
                {
                    TaskDialog.Show("Warning", "Selected element does not have a valid bounding box.");
                    continue;
                }
                // Display the bounding box dimensions
                double length = bbox.Max.X - bbox.Min.X;
                double width = bbox.Max.Y - bbox.Min.Y;
                double height = bbox.Max.Z - bbox.Min.Z;
                TaskDialog.Show("Selected Element Bounding Box", $"Length: {length}, Width: {width}, Height: {height}");
            }
            return Result.Succeeded;
        }
    }

