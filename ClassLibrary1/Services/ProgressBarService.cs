using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMBOX.Revit.Tuna.Services
{
    public class ProgressBarService : IProgressBarService
    {
        Views.ProgressBarDialog _dialog;
        public void Start(int maximum)
        {
            var dialog = SimpleIoc.Default.GetInstance<Views.ProgressBarDialog>();
            dialog.DataContext = SimpleIoc.Default.GetInstanceWithoutCaching<ViewModels.ProgressBarDialogViewModel>();
            dialog.Show();
            Messenger.Default.Send<int>(maximum, Contacts.Tokens.ProgressBarMaximum);
        }

        public void Stop()
        {
            Messenger.Default.Unregister(_dialog.DataContext);
            _dialog.Close();
        }
    }
}
