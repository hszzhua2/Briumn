using BIMBOX.Revit.Tuna.Helpers.MessageWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMBOX.Revit.Tuna.Helpers
{
    class MessageWindows
    {
        /// <summary>
        /// Method to display custom alert messages to user
        /// </summary>
        /// <param name="windowTitle"></param>
        /// <param name="errorMessage"></param>
        public static void AlertMessage(string windowTitle, string errorMessage)
        {
            using (ErrorMessageWPF customWindow = new ErrorMessageWPF(windowTitle, errorMessage))
            {
                // Place message at the top of Revit 
                System.Windows.Interop.WindowInteropHelper helper = new System.Windows.Interop.WindowInteropHelper(customWindow);
                helper.Owner = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;

                customWindow.ShowDialog();
            }
        }
    }
}
