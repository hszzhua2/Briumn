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

namespace BIMBOX.Revit.Tuna.Helpers.UserControls.LabelComboBox
{
    /// <summary>
    /// Interaction logic for UCComboBox.xaml
    /// </summary>
    public partial class UCComboBox : System.Windows.Controls.UserControl
    {
        /// <summary>
        /// Get or Set text in TextBlock
        /// </summary>
        public string Label
        {
            get => (string)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }

        /// <summary>
        /// Identify the Label dependency property
        /// </summary>
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string),
              typeof(UCComboBox), new PropertyMetadata(""));

        /// <summary>
        /// Get or Set value displayed in TextBox
        /// </summary>
        public object Value
        {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        /// <summary>
        /// Identify the Value dependency property
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(object),
              typeof(UCComboBox), new PropertyMetadata(null));

        public UCComboBox()
        {
            InitializeComponent();
        }
    }
}
