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
using NetworkHelper;

namespace wpf_regisztraciosForm
{
    /// <summary>
    /// Interaction logic for LogInWindow.xaml
    /// </summary>
    public partial class LogInWindow : Window
    {
        List<User> users = new List<User>();
        public LogInWindow()
        {
            InitializeComponent();
            Listafeltolt();
        }

        private void Listafeltolt()
        {
            string url = "http://localhost:3000/all";
            users = Backend.GET(url).Send().As<List<User>>();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void LogIn_Click(object sender, RoutedEventArgs e)
        {
            string email = userEmail.Text.Trim();
            string password = userPwd.Password.Trim();
            var user = users.FirstOrDefault(x=>x.email==email);
            bool isExistEmail = users.Any(x => x.email == email);

            if (userEmail.Text=="admin"&&userPwd.Password=="admin")
            {
                AdminWindow adminWindow = new AdminWindow();
                adminWindow.Show();
                this.Close();
                return;
            }

            if (isExistEmail)
            {
                if (user.jelszo == password)
                {
                    ActiveUser.BejelentkezettUser = user;
                    UserDataWindow userDataWindow = new UserDataWindow();
                    userDataWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Hibás jelszó!", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Nincs ilyen regisztrált felhasználó!", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }
    }
}
