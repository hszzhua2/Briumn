using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using System.Windows;
using Autodesk.Revit.DB;

namespace BIMBOX.Revit.Tuna.DockablePanes
{
    public class FramworkElementCreator : IFrameworkElementCreator
    {
        public FrameworkElement CreateFrameworkElement()
        {
            Views.DockablePaneControl dockablePaneControl = new Views.DockablePaneControl()
            {
                DataContext = new ViewModels.DockablePaneControlViewModel()
            };
            return dockablePaneControl;
        }
    }
}
