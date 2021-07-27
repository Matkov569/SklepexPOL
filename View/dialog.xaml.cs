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
using System.Windows.Shapes;

namespace SklepexPOL.View
{
    /// <summary>
    /// Logika interakcji dla klasy dialog.xaml
    /// </summary>
    public partial class dialog : Window
    {
        public dialog()
        {
            InitializeComponent();
        }
        public string ResponseLogin
        {
            get { return Login.Text; }
            set { Login.Text = value; }
        }
        public string ResponsePassword
        {
            get { return Password.Text; }
            set { Password.Text = value; }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
