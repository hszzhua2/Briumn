using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using BIMBOX.Revit.Toolkit.Extension;
using System.IO;
using Application = Autodesk.Revit.ApplicationServices.Application;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace BIMBOX.Revit.Tuna.Commands
{
    [Transaction(TransactionMode.Manual)]
    class Export2glTFCommand : IExternalCommand
    {
        public void ExportView3D(View3D view3d, string filename, string directory)
        {
            Document doc = view3d.Document;
            // Use our custom implementation of IExportContext as the exporter context.
            glTFExportContext ctx = new glTFExportContext(doc, filename, directory);
            // Create a new custom exporter with the context.
            CustomExporter exporter = new CustomExporter(doc, ctx);

            exporter.ShouldStopOnError = true;
            exporter.Export(view3d);
        }
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            View3D view = doc.ActiveView as View3D;
            if (view == null)
            {
                TaskDialog.Show("Briumn提示", "当前视图不是3D视图，请切换到3D视图导出^-^");
                return Result.Failed;
            }
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.FileName = "NewProject"; // default file name
            fileDialog.DefaultExt = ".gltf"; // default file extension

            bool? dialogResult = fileDialog.ShowDialog();
            if (dialogResult == true)
            {
                string filename = fileDialog.FileName;
                string directory = Path.GetDirectoryName(filename) + "\\";

                ExportView3D(view, filename, directory);
                string messageText = $"glTF文件成功导出到=> {directory}";
                TaskDialog.Show("Briumn提示", messageText);
            }
            return Result.Succeeded;
        }
    }
}
