using Autodesk.Revit.DB;
using BIMBOX.Revit.Entity;
using BIMBOX.Revit.Tuna.ViewModels;
using GalaSoft.MvvmLight.Messaging;
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

namespace BIMBOX.Revit.Tuna.Views
{
    /// <summary>
    /// Materials.xaml 的交互逻辑
    /// </summary>
    public partial class Materials : System.Windows.Window
    {

        //private Document _document;
        public Materials(Document document)
        {
            InitializeComponent();
            this.DataContext = new MaterialsViewModel(document);
            Messenger.Default.Register<bool>(this, Contacts.Tokens.MaterialsDialog, CloseWindow);
            this.Unloaded += Materials_Unloaded;
                

            /*FilteredElementCollector elements = new FilteredElementCollector(document).OfClass(typeof(Material));

            foreach (var item in elements)
            {
                this.materials.Items.Add(item);
            }*/

        }

      
        private void Materials_Unloaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Unregister<bool>(this, CloseWindow);
        }

        private void CloseWindow(bool result)
        {
            this.DialogResult = result;
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        /*private void btn_deleteMaterials_Click(object sender, RoutedEventArgs e)
        {
            using (Transaction transaction = new Transaction(_document, "删除材质"))
            {
                transaction.Start();
                for (int i = materials.SelectedItems.Count - 1; i >= 0; i--)
                {
                    _document.Delete((materials.SelectedItems[i] as Element).Id);
                    this.materials.Items.Remove(materials.SelectedItems[i]);
                }
                transaction.Commit();
            }
        }

        private void btn_submit_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }*/
    }
}
  