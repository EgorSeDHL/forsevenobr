using FoodDelivery.Courier;
using FoodDelivery.FoodDeliveryDBDataSetTableAdapters;
using System.Windows;
using System.Windows.Controls;

namespace FoodDelivery
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        UsersTableAdapter users = new UsersTableAdapter();
        public Login()
        {
            InitializeComponent();
        }

        private void EnterBtn_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            var userTable = users.GetData();

            bool userFound = false;
            /*   UserWindow userWindow = new UserWindow(1);
               userWindow.Show();*/
            foreach (var row in userTable)
            {

                string storedUsername = row.username;
                string storedPassword = row.password;

                if (storedUsername == username)
                {
                    userFound = true;

                    if (storedPassword == password)
                    {

                        CheckRole(row, row.user_id);
                    }
                    else
                    {
                        MessageBox.Show("НЕ ВЕРНЫЙ ПАРОЛЬ!!!");
                        return;
                    }
                }
            }

            if (!userFound)
            {
                MessageBox.Show("НЕ НАШЕЕЕЛ ЮЗЕРА!!!");
            }

        }
        private void CheckRole(FoodDeliveryDBDataSet.UsersRow row, int userID)
        {
            if (row.role_id == 1)
            {
                MessageBox.Show("Admin");

            }
            if (row.role_id == 2)
            {
                UserWindow userWindow = new UserWindow(userID);
                userWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Курьер");
                CourierWindow courierWindow = new CourierWindow(userID);
                courierWindow.Show();
                this.Close();
            }

        }
    }
}
