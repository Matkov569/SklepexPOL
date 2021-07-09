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
        }
        public void gameStart()
        {
            Console.WriteLine("gameStart");
            menuContent.Visibility = Visibility.Collapsed;
            gameContent.Visibility = Visibility.Visible;
        }
        public void goMenu()
        {
            Console.WriteLine("goMenu");
            menuContent.Visibility = Visibility.Visible;
            gameContent.Visibility = Visibility.Collapsed;
        }
    }
}
