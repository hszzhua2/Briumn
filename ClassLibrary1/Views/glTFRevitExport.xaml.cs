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
using Autodesk.Revit.DB;

namespace BIMBOX.Revit.Tuna.Views
{
    /// <summary>
    /// glTFRevitExport.xaml 的交互逻辑
    /// </summary>
    public partial class glTFRevitExport : Window
    {
        public glTFRevitExport(Document doc)
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            btnOk.Click += (sender, e) => btnOk_Click(sender, e, doc);
        }

        private void btnOk_Click(object sender, RoutedEventArgs e, Document doc)
        {
            Close();
        }
    }
}
