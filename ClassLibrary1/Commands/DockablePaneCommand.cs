
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMBOX.Revit.Tuna.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class DockablePaneCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var id = DockablePanes.DockablePaneProvider.Id;
            if (!DockablePane.PaneExists(id))
            {
                return Result.Cancelled;
            }

            DockablePane pane = commandData.Application.GetDockablePane(id);
            if (pane.IsShown())
            {
                pane.Hide();
            }
            else
            {
                pane.Show();
            }
            return Result.Succeeded;    
        }
    }
}
