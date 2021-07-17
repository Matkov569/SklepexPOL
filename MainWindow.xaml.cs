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

namespace SklepexPOL
{
    using ViewModel.BaseClass;
    using ViewModel;
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.menuContent.Content = new View.menu();
            this.gameContent.Content = new View.game();
            this.dateContent.Content = new View.data();
            this.NGContent.Content = new View.newgame();
            MainViewModel vm = this.DataContext as MainViewModel;
            vm.Hello.Execute(null);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainViewModel vm = this.DataContext as MainViewModel;
            vm.GoodBye.Execute(null);
        }
    }
}
