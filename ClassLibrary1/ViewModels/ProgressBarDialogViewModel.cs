using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMBOX.Revit.Tuna.ViewModels
{
    public class ProgressBarDialogViewModel : ViewModelBase
    {
        public ProgressBarDialogViewModel() 
        {
            MessengerInstance.Register<int>(this, Contacts.Tokens.ProgressBarMaximum, (max) =>
            {
                Maximum = max;
            });

            MessengerInstance.Register<string>(this, Contacts.Tokens.ProgressBarTile, (title) =>
            {
                System.Windows.Forms.Application.DoEvents();
                Value++;
                Title = $"{Value}/{Maximum}_{title}";
                
            });
        }
        private int _maximum;
        private int _value;
        private string _title;

        public int Maximum
        {
            get { return _value; }
            set { _value = value; RaisePropertyChanged(); }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; RaisePropertyChanged(); }
        }

        public int Value
        { 
            get { return _value; }
            set { _value = value;  RaisePropertyChanged(); } 
        }
    }
}
