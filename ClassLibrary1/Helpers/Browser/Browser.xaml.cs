using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace BIMBOX.Revit.Tuna.Helpers.Browser.ViewModels
{
    public partial class Browser : Window, IDisposable
    {
        /// <summary>
        /// Method to call window
        /// </summary>
        public Browser()
        {
            InitializeComponent();

            this.DataContext = new BrowserStructureViewModel();
        }

        /// <summary>
        /// Implement Idisposable interface
        /// </summary>
        public void Dispose()
        {
            this.Close();
        }

        /// <summary>
        /// Method for cancel button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Dispose();
        }

        /// <summary>
        /// String to store user's selected path
        /// </summary>
        public string selectedPath = null;

        /// <summary>
        /// Method for ok click to return selected path
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OK_Click(object sender, RoutedEventArgs e)
        {
            BrowserItemViewModel selectedFolder = FolderView.SelectedItem as BrowserItemViewModel;

            if (selectedFolder != null)
            {
                // Assign selected path to variable for use in main program
                selectedPath = selectedFolder.FullPath;
            }

            this.Dispose();
        }
    }
}