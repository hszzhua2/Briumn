using Autodesk.Revit.Creation;
using Autodesk.Revit.DB;
using BIMBOX.Revit.Entity;
using BIMBOX.Revit.Toolkit.Extension;
using BIMBOX.Revit.Tuna.Views;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BIMBOX.Revit.Tuna.ViewModels
{
    public class MaterialsViewModel
    {

        private Autodesk.Revit.DB.Document _document;

        public MaterialsViewModel(Autodesk.Revit.DB.Document document) 
        {
            this._document = document;


            _materials = new ObservableCollection<BOX_Material>();
            QueryElement();
        }
        //事件命令
        private RelayCommand<IList> _deleteElementsCommand;
        private RelayCommand _queryElementsCommand;
        private string _keyword;
        //中继命令
        private void DeleteElements(IList selectedElements)
        {
            _document.NewTransaction("删除材质", ()=>
            {
                for (int i = selectedElements.Count - 1; i >= 0; i--)
                {
                    BOX_Material material = selectedElements[i] as BOX_Material;
                    _document.Delete(material.Material.Id);
                    Materials.Remove(material);
                }
            });
        }

        private ObservableCollection<BOX_Material> _materials;
        private RelayCommand _submitCommand;
        private RelayCommand _sumbitCommand;
        private RelayCommand<IList> _editCommand;
        public ObservableCollection<BOX_Material> Materials
        {
            get { return _materials; }
            set { _materials = value;}
        }
        
        public RelayCommand QueryElementsCommand { get => _queryElementsCommand ??= new RelayCommand(QueryElement); }
        public RelayCommand SubmitCommand { get => _sumbitCommand ??= new RelayCommand(Submit); }

        

        

        private void Submit()
        {
            Messenger.Default.Send(true, Contacts.Tokens.MaterialsDialog);
        }

        private bool CanQueryElements()
        {
            return string.IsNullOrEmpty(_keyword);
        }

        private void QueryElement()
        {
            Materials.Clear();
            FilteredElementCollector elements = new FilteredElementCollector(_document).OfClass(typeof(Material));
            var materials = new ObservableCollection<BOX_Material>(elements.ToList()
                .ConvertAll(x => new BOX_Material(x as Material))
                .Where(e => string.IsNullOrEmpty(Keyword) || e.Name.Contains(Keyword)));

            foreach (var item in materials)
            {
                Materials.Add(item);
            }

        }

        public string Keyword
        {
            get { return _keyword; }
            set { _keyword = value; /*_queryElementsCommand.RaiseCanExecuteChanged();*/ }
        }


        public RelayCommand<IList> DeleteElementsCommand 
        { 
            get=> _deleteElementsCommand??new RelayCommand<IList>(DeleteElements); 
        }

        
        //命令没做完
    }
}
