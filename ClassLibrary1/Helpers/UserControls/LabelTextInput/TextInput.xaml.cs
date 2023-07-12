using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BIMBOX.Revit.Tuna.Helpers.UserControls.LabelTextInput
{

    #region DPs
    /// <summary>
    /// Interaction logic for TextInput.xaml
    /// </summary>
    public partial class TextInput : System.Windows.Controls.UserControl
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
              typeof(TextInput), new PropertyMetadata(""));

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
              typeof(TextInput), new PropertyMetadata(null));

        /// <summary>
        /// Get or Set the orientation of the container of the TextBlock and TextBox
        /// </summary>
        public System.Windows.Controls.Orientation Orientation
        {
            get
            {
                System.Windows.Controls.Orientation? o = GetValue(OrientationProperty) as System.Windows.Controls.Orientation?;
                return o.HasValue ? o.Value : System.Windows.Controls.Orientation.Horizontal;
            }
            set => SetValue(OrientationProperty, value);
        }

        /// <summary>
        /// Identify the Orientation dependency property
        /// </summary>
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(System.Windows.Controls.Orientation),
                typeof(TextInput), new PropertyMetadata(System.Windows.Controls.Orientation.Horizontal));

        #endregion

        /// <summary>
        /// Initialize component
        /// </summary>
        public TextInput()
        {
            InitializeComponent();
        }
    }
}
