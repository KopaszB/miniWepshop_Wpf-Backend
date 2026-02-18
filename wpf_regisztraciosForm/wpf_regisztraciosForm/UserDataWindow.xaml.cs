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

namespace wpf_regisztraciosForm
{
    /// <summary>
    /// Interaction logic for UserDataWindow.xaml
    /// </summary>
    public partial class UserDataWindow : Window
    {
        public UserDataWindow()
        {
            InitializeComponent();
        }

        private void MenuItemQuit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuItemData_Click(object sender, RoutedEventArgs e)
        {
            tbx_name.Text = ActiveUser.BejelentkezettUser.nev;
            tbx_email.Text = ActiveUser.BejelentkezettUser.email;
            tbx_date.Text = ActiveUser.BejelentkezettUser.szul_datum;

        }
    }
}
