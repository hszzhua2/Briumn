using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace BIMBOX.Revit.Tuna.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class SwitchBackGround : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            UIApplication uiApp = commandData.Application;

            using (Transaction trans = new Transaction(doc, "Switch Background Color"))
            {
                trans.Start();

                // Check current background color
                if (uiApp.Application.BackgroundColor.Red == 255 && uiApp.Application.BackgroundColor.Green == 255 && uiApp.Application.BackgroundColor.Green == 255)
                {
                    uiApp.Application.BackgroundColor = new Autodesk.Revit.DB.Color(33, 40, 48);
                }
                else
                {
                    uiApp.Application.BackgroundColor = new Autodesk.Revit.DB.Color(255, 255, 255);
                }

                trans.Commit();
            }

            return Result.Succeeded;
        }
    }
}