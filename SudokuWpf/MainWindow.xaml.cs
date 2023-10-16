using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using log4net;

namespace SudokuWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static ILog Log { get; } = LogManager.GetLogger(typeof(MainWindow));
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(Object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show(this, "On Click","Title", MessageBoxButton.YesNo);
            // Console.WriteLine(messageBoxResult);
            Trace.WriteLine(messageBoxResult);
            Log.Debug(messageBoxResult);
        }
    }
}