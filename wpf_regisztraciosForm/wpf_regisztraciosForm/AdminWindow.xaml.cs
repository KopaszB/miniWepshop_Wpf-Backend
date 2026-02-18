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
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        List<User> users = new List<User>();

        public AdminWindow()
        {
            InitializeComponent();
            DataGridFeltolt();
        }
        private void DataGridFeltolt()
        {
            string url = "http://localhost:3000/all";
            users = Backend.GET(url).Send().As<List<User>>();
            usersTable.ItemsSource = users;
        }

        private void Btn_add_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(userName.Text)||string.IsNullOrWhiteSpace(userEmail.Text)||string.IsNullOrWhiteSpace(userDate.Text)||string.IsNullOrWhiteSpace(userPassword.Text))
            {
                MessageBox.Show("Hiba!", "Minden mezőt ki kell tölteni!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var newUser = new {
                nev = userName.Text,
                email= userEmail.Text,
                szul_datum = userDate.Text,
                jelszo = userPassword.Text
            };

            string url = "http://localhost:3000/users";

            try
            {
                Backend.POST(url).Body(newUser).Send();
                MessageBox.Show("Sikeres regisztráció","Felhasználó sikeresen hozzáadva!",MessageBoxButton.OK,MessageBoxImage.Information);
                userName.Text = "";
                userEmail.Text = "";
                userDate.Text = "";
                userPassword.Text = "";
                DataGridFeltolt();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba!", "Hiba történt a hozzáadás során!\n"+ex.Message, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Btn_modify_Click(object sender, RoutedEventArgs e)
        {
            User selectedUser = usersTable.SelectedItem as User;
            if (selectedUser==null)
            {
                MessageBox.Show("Válassz ki egy felhasználót!");
                return;
            }
            //egyszerű validáció
            if (string.IsNullOrWhiteSpace(userName.Text) || string.IsNullOrWhiteSpace(userEmail.Text) || string.IsNullOrWhiteSpace(userDate.Text) || string.IsNullOrWhiteSpace(userPassword.Text))
            {
                MessageBox.Show("Hiba!", "Minden mezőt ki kell tölteni!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            //frissített adatok összeállítása
            var newUser = new
            {
                nev = userName.Text,
                email = userEmail.Text,
                szul_datum = userDate.Text,
                jelszo = userPassword.Text
            };
            string url = $"http://localhost:3000/users/{selectedUser.id}";
            try
            {
                Backend.PUT(url).Body(newUser).Send();
                MessageBox.Show("Sikeres a módosítás!");
                DataGridFeltolt();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba!", "Hiba történt a módosítás során!\n" + ex.Message, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Btn_delete_Click(object sender, RoutedEventArgs e)
        {
            User selectedUser = usersTable.SelectedItem as User;
            if (selectedUser == null)
            {
                MessageBox.Show("Válassz ki egy felhasználót!");
                return;
            }

            MessageBoxResult confirm = MessageBox.Show($"Biztosan törlöd ezt a felhasználót? {selectedUser.nev}\n{selectedUser.email}","Felhasználó törlése",MessageBoxButton.YesNo,MessageBoxImage.Warning);

            if (confirm!=MessageBoxResult.Yes)
            {
                return;
            }

            string url = $"http://localhost:3000/users/{selectedUser.id}";

            try
            {
                Backend.DELETE(url).Send();
                MessageBox.Show("Felhasználó törlése megtörtént","Törlés",MessageBoxButton.OK,MessageBoxImage.Information);
                DataGridFeltolt();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba történt a törlés során!" + ex.Message,"Hiba!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private void Btn_refresh_Click(object sender, RoutedEventArgs e)
        {
            userName.Text = "";
            userEmail.Text = "";
            userDate.Text = "";
            userPassword.Text = "";
            DataGridFeltolt();
        }

        private void LinkLogin_Click(object sender, RoutedEventArgs e)
        {
            LogInWindow logInWindow = new LogInWindow();
            logInWindow.Show();
            this.Close();
        }

        private void LinkReg_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void LinkExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void UsersTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            User selectedUser = usersTable.SelectedItem as User;
            if (selectedUser!=null)
            {
                userName.Text = selectedUser.nev;
                userEmail.Text = selectedUser.email;
                userDate.Text = selectedUser.szul_datum;
                userPassword.Text = selectedUser.jelszo;
            }
        }
    }
}
