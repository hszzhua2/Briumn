using GalaSoft.MvvmLight.Command;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace BIMBOX.Revit.Tuna.ViewModels
{
    public class PipTurnOverViewModel : INotifyPropertyChanged
    {
        private double hValue;

        public double HValue
        {
            get { return hValue; }
            set
            {
                if (hValue != value)
                {
                    hValue = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand ExecuteCommand { get; }

        public PipTurnOverViewModel()
        {
            ExecuteCommand = new RelayCommand(Execute);
        }

        private void Execute()
        {
            try
            {
                // Access the value of h from the view
                double h = HValue;

                // Perform the necessary logic here using the value of h
                // ...

                // Display a success message to the user
                MessageBox.Show("Command executed successfully.");
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the execution
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
