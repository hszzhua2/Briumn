﻿using System;
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
    /// FloorFinishWPF.xaml 的交互逻辑
    /// </summary>
    public partial class FloorFinishWPF : Window, IDisposable
    {
        public FloorFinishWPF(Autodesk.Revit.UI.ExternalCommandData commandData)
        {
            InitializeComponent();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
