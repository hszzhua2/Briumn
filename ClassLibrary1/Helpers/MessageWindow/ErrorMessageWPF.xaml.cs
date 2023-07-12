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

namespace BIMBOX.Revit.Tuna.Helpers.MessageWindow
{
    /// <summary>
    /// ErrorMessageWPF.xaml 的交互逻辑
    /// </summary>
    public partial class ErrorMessageWPF : Window, IDisposable
    {
        private string _windowTitle;
        public string WindowTitle { get { return _windowTitle; } set { _windowTitle = value; } }

        private string _errorMessage;
        public string ErrorMessage { get { return _errorMessage; } set { _errorMessage = value; } }
        public ErrorMessageWPF(string windowTitle, string errorMessage)
        {
            InitializeComponent();
            this.Title = windowTitle;
            TextMessage.Text = errorMessage;
        }

        public void Dispose()
        {
            this.Close();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            this.Dispose();
        }
    }
}
