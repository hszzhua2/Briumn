using BIMBOX.Revit.Tuna.Helpers.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BIMBOX.Revit.Tuna.Helpers.UserControls.OKCancel.ViewModel
{
    class OKCancelViewModel : ViewModelBase
    {
        private readonly ICommand _okExecute;
        public ICommand OKExecute
        {
            get { return _okExecute; }
        }

        public void OKButtonPress(string message)
        {

        }

        public OKCancelViewModel()
        {
            _okExecute = new RelayCommand(() => OKButtonPress("message"));
        }
    }
}
