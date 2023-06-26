using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// MainUIPanel.xaml 的交互逻辑
    /// </summary>
    public partial class MainUIPanel : Window
    {
        public MainUIPanel()
        {
            InitializeComponent();
            DataContext = this;
            UpdateCurrentTime();
        }
        private string _currentTime;

        public string CurrentTime
        {
            get { return _currentTime; }
            set
            {
                _currentTime = value;
                OnPropertyChanged();
            }
        }

        private void Button_ClickClose(object sender, RoutedEventArgs e)
        {
            this.Close(); //关闭窗口
        }
        private void UpdateCurrentTime()
        {
            CurrentTime = DateTime.Now.ToString("HH:mm:ss");
            // 每秒更新一次时间
            Dispatcher.BeginInvoke(new Action(UpdateCurrentTime), null, TimeSpan.FromSeconds(1));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
