using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace BIMBOX.Revit.Tuna.Views
{
    /// <summary>
    /// FaceConfig.xaml 的交互逻辑
    /// </summary>
    public partial class FaceConfig : Window
    {
        public WallType SelectedWallType { get; set; }
        public FaceConfig(List<WallType> wallTypes)
        {
            InitializeComponent();
            var vm = new ViewModel();
            wallTypes.ForEach(x => vm.WallTypes.Add(x));
            DataContext = vm;
        }

        private void cWallType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var wallTpye = e.AddedItems?.Count > 0 ? e.AddedItems[0] : null;
            if (wallTpye != null)
                SelectedWallType = (WallType)wallTpye;
        }

        private void SwitchBackground(object sender, RoutedEventArgs e)
        {
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedWallType != null)
            {
                DialogResult = true;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("请选择面类型");
                DialogResult = false;
            }
        }


    }
    public class ViewModel
    {
        public ObservableCollection<WallType> WallTypes { get; }= new ObservableCollection<WallType>();
    }



}
