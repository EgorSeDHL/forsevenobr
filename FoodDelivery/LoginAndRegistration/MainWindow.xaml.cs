using FoodDelivery.FoodDeliveryDBDataSetTableAdapters;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace FoodDelivery
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        UsersTableAdapter users = new UsersTableAdapter();
        RolesTableAdapter roles = new RolesTableAdapter();
        Menu_ItemsTableAdapter menu = new Menu_ItemsTableAdapter();
        OrdersTableAdapter orders = new OrdersTableAdapter();
        PaymentsTableAdapter payments = new PaymentsTableAdapter();
        Order_ItemsTableAdapter order_items = new Order_ItemsTableAdapter();
        RestaurantsTableAdapter restaurants = new RestaurantsTableAdapter();
        ReviewsTableAdapter reviews = new ReviewsTableAdapter();
        public MainWindow()
        {
            InitializeComponent();

        }
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;
            string email = EmailTextBox.Text;
            string phone = PhoneTextBox.Text;
            string addres = AddresTextBox.Text;

            // Логика добавления пользователя в базу данных



            if (VaildationData(username, password, email, phone, addres))
            {
                users.InsertQuery(username, password, email, phone, addres, 2);

            }

        }
        public static bool VaildationData(string username, string password, string email, string phone, string addres)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(addres))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return false;
            }
            if (password.Length < 8)
            {
                MessageBox.Show("Пароль должен быть минимум 8 символов");
                return false;

            }

            if (!IsValidEmail(email))
            {
                MessageBox.Show("Введите корректный email");
                return false;

            }
            if (!IsValidPhoneNumber(phone))
            {
                MessageBox.Show("Введите корректный телефон");
                return false;

            }
            return true;

        }
        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            string pattern = @"^8\d{10}$";
            return Regex.IsMatch(phoneNumber, pattern);
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }

        private void LogiButton_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }
    }
}
