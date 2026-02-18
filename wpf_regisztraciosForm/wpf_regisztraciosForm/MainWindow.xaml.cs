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
using NetworkHelper;
using System.Text.RegularExpressions;

namespace wpf_regisztraciosForm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<User> users = new List<User>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void UserEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            string email = userEmail.Text.Trim();
            if (IsValidEmail(email))
            {
                userEmail.BorderBrush = Brushes.Green;
            }
            else
            {
                userEmail.BorderBrush = Brushes.Red;
                MessageBox.Show("Helytelen email cím!", "Email cím hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email,pattern);
        }

        private void UserDate_LostFocus(object sender, RoutedEventArgs e)
        {
            if (userDate.SelectedDate == null)
            {
                userDate.BorderBrush = Brushes.Red;
                return;
            }
            DateTime date = userDate.SelectedDate.Value;
            if (date>DateTime.Today)
            {
                MessageBox.Show("Helytelen dátum!", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Warning);
                userDate.SelectedDate = null;
                userDate.BorderBrush = Brushes.Red;
            }
            else
            {
                userDate.BorderBrush = Brushes.Green;
            }
            

        }

        private void UserPwd1_LostFocus(object sender, RoutedEventArgs e)
        {
            string password = userPwd1.Password;
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$";
            if (Regex.IsMatch(password,pattern))
            {
                userPwd1.BorderBrush = Brushes.Green;
            }
            else
            {
                userPwd1.BorderBrush = Brushes.Red;
                MessageBox.Show("Tartalmazia kell számot, kis- és nagy betűt, legalább 8 karakter hosszúságú kell, hogy legyen!", "Helytelen jelszó!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void UserPwd2_LostFocus(object sender, RoutedEventArgs e)
        {
            if (userPwd1.Password==userPwd2.Password)
            {
                userPwd2.BorderBrush = Brushes.Green;
            }
            else
            {
                userPwd2.BorderBrush = Brushes.Red;
                MessageBox.Show("Nem egyezik a megadott jelszóval!", "Különböző jelszavak!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Registration_Click(object sender, RoutedEventArgs e)
        {
            string email = userEmail.Text.Trim().ToLower();
            bool emailExist = users.Any(x=>x.email.Trim().ToLower()==email);
            if (userName.Text!=null&&userEmail.Text!=null&&userDate.SelectedDate.Value!=null&&userPwd1.Password!=null&&conditions.IsChecked==true)
            {
                if (!emailExist)
                {
                    try
                    {
                        string url = "http://localhost:3000/users";
                        User newUser = new User()
                        {
                            nev = userName.Text.Trim(),
                            email = userEmail.Text.Trim(),
                            szul_datum = userDate.SelectedDate.ToString(),
                            jelszo = userPwd1.Password.Trim()
                        };
                        string response = Backend.POST(url).Body(newUser).Send().As<string>();
                        users.Add(newUser);
                        MessageBox.Show("Sikeres regisztráció!","Gratulálunk!", MessageBoxButton.OK, MessageBoxImage.Information);
                        mindenAlaphelyzetbe();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Hiba: {ex}");
                    }
                }
                else
                {
                    MessageBox.Show("A megadott email cím már létezik!", "Figyelmeztetés!",MessageBoxButton.OK,MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Töltsd ki az összes mezőt és fogadd el a feltételeket!", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void mindenAlaphelyzetbe()
        {
            userName.Text = "";
            userEmail.Text = "";
            userDate.SelectedDate = null;
            userPwd1.Password = "";
            userPwd2.Password = "";
            conditions.IsChecked = false;
            userName.BorderBrush = Brushes.LightGray;
            userEmail.BorderBrush = Brushes.LightGray;
            userDate.BorderBrush = Brushes.LightGray;
            userPwd1.BorderBrush = Brushes.LightGray;
            userPwd2.BorderBrush = Brushes.LightGray;
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            LogInWindow logInWindow = new LogInWindow();
            logInWindow.Show();
            this.Hide();
        }
    }
}
