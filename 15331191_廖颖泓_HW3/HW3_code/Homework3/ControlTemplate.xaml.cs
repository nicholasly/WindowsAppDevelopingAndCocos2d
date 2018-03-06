using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Homework3.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Homework3
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ControlTemplate : UserControl
    {
        public Models.TodoItem TodoItem { get { return DataContext as Models.TodoItem; } }
        public ControlTemplate()
        {
            this.InitializeComponent();
            this.DataContextChanged += (s, e) => Bindings.Update();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            LINE.StrokeThickness = 2;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            LINE.StrokeThickness = 0;
        }
    }
}
