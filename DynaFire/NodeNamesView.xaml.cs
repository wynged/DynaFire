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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xaml;

namespace DynaFire
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class NodeNamesView : Window
    {
        DynaFireExtension model;
        public NodeNamesView(DynaFireExtension dynaFire)
        {
            model = dynaFire;
            DataContext = dynaFire;
            InitializeComponent();
        }

        private void Button_Click_Save(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_Cancel(object sender, RoutedEventArgs e)
        {
            model.ClearKeys();
            model.ReadFile();
        }
    }
}
