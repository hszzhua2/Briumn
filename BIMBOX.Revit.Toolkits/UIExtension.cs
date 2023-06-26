using Autodesk.Revit.UI;
using System.Windows;
using System.Windows.Media.Imaging;


namespace Tuna.Revit.Extension.Revit
{
    public static class UIExtension
    {
        public static RibbonPanel CreateButton<T>(this RibbonPanel panel, Action<PushButtonData> action)
        {
            Type type = typeof(T);
            string name = $"btn_{type.Name}";
            PushButtonData pushButtonData = new PushButtonData(name, name, type.Assembly.Location, type.FullName);
            action.Invoke(pushButtonData);
            panel.AddItem(pushButtonData);
            return panel;
        }

        /*public static BitmapSource ConvertToBitmapSource(this System.Drawing.Bitmap bitmap)
        {
            if (bitmap == null)
            {
                throw new ArgumentNullException(nameof(bitmap), "bitmap can not be null");
            }

            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bitmap.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
        }*/
    }
}
