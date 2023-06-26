using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BIMBOX.Revit.Tuna.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class RoomBoundaryLocation : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Document doc = uiDoc.Document;

            // Create a filtered element collector to get all room elements
            FilteredElementCollector roomCollector = new FilteredElementCollector(doc);
            roomCollector.OfCategory(BuiltInCategory.OST_Rooms).WhereElementIsNotElementType();

            // Create Excel workbook and worksheet
            IWorkbook workbook = new XSSFWorkbook();
            ISheet roomBoundSheet = workbook.CreateSheet("Room Boundaries");
            int roomBoundRowCount = 0;

            // Set title row
            IRow titleRow = roomBoundSheet.CreateRow(roomBoundRowCount++);
            titleRow.CreateCell(0).SetCellValue("RoomName");
            titleRow.CreateCell(1).SetCellValue("StartPointX");
            titleRow.CreateCell(2).SetCellValue("StartPointY");
            titleRow.CreateCell(3).SetCellValue("StartPointZ");
            titleRow.CreateCell(4).SetCellValue("EndPointX");
            titleRow.CreateCell(5).SetCellValue("EndPointY");
            titleRow.CreateCell(6).SetCellValue("EndPointZ");

            foreach (Element room in roomCollector)
            {
                SpatialElementBoundaryOptions options = new SpatialElementBoundaryOptions();
                options.SpatialElementBoundaryLocation = SpatialElementBoundaryLocation.Finish;

                // Get the room boundary segments
                IList<IList<BoundarySegment>> segments = ((SpatialElement)room).GetBoundarySegments(options);

                // Loop through each boundary segment
                foreach (IList<BoundarySegment> segmentList in segments)
                {
                    foreach (BoundarySegment segment in segmentList)
                    {
                        // Get the start point and end point of the boundary segment
                        XYZ startPoint = segment.GetCurve().GetEndPoint(0);
                        XYZ endPoint = segment.GetCurve().GetEndPoint(1);

                        // Add the boundary line to the Excel worksheet
                        IRow roomBoundDataRow = roomBoundSheet.CreateRow(roomBoundRowCount++);
                        roomBoundDataRow.CreateCell(0).SetCellValue(room.Name);
                        roomBoundDataRow.CreateCell(1).SetCellValue(startPoint.X);
                        roomBoundDataRow.CreateCell(2).SetCellValue(startPoint.Y);
                        roomBoundDataRow.CreateCell(3).SetCellValue(startPoint.Z);
                        roomBoundDataRow.CreateCell(4).SetCellValue(endPoint.X);
                        roomBoundDataRow.CreateCell(5).SetCellValue(endPoint.Y);
                        roomBoundDataRow.CreateCell(6).SetCellValue(endPoint.Z);
                    }
                }
            }

            // Save Excel file
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = Path.Combine(desktopPath, "RoomBoundaries.xlsx");

            int i = 1;
            string finalFilePath = filePath;
            while (File.Exists(finalFilePath))
            {
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                finalFilePath = Path.Combine(desktopPath, $"{fileName}_{i++}{extension}");
            }

            using (FileStream fileStream = new FileStream(finalFilePath, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fileStream);
            }

            // Display success message
            TaskDialog.Show("Briumn提示", $"房间边界信息已成功保存至 {Path.GetFileName(finalFilePath)} !");

            return Result.Succeeded;
        }
    }
}
