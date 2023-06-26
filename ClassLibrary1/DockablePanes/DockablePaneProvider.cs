using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMBOX.Revit.Tuna.DockablePanes
{
    public class DockablePaneProvider: IDockablePaneProvider
    {
        public static DockablePaneId Id => new DockablePaneId(new Guid("6B8D269E-F22F-4E3C-A292-1C66678A5A40"));

        public void SetupDockablePane(DockablePaneProviderData data)
        {
            #region Default Mode
            var dockablePaneState1 = new DockablePaneState()
            {
              DockPosition=DockPosition.Right
            };
            #endregion

            #region Floating Mode
            var dockablePaneState2 = new DockablePaneState()
            {
              DockPosition=DockPosition.Floating,
            };
            dockablePaneState2.SetFloatingRectangle(new Autodesk.Revit.DB.Rectangle(100, 100, 500, 500));
            #endregion

            #region Tabbed Mode
            var dockablePaneState3 = new DockablePaneState()
            {
              DockPosition=DockPosition.Tabbed,
            };
            dockablePaneState3.TabBehind = Autodesk.Revit.UI.DockablePanes.BuiltInDockablePanes.ProjectBrowser;
            #endregion

            data.FrameworkElementCreator = new FramworkElementCreator();
            data.InitialState = dockablePaneState1;
            data.VisibleByDefault = false;
        }
    }

}
