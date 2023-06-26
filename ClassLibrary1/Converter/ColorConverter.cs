using Autodesk.Revit.DB.Architecture;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BIMBOX.Revit.Tuna.Converter
{
    public class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Autodesk.Revit.DB.Color color)
            {
               return ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(color.Red,color.Green, color.Blue));
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
