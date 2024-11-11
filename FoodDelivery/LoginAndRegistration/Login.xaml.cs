using FoodDelivery.Admin;
using FoodDelivery.Courier;
using FoodDelivery.FoodDeliveryDBDataSetTableAdapters;
using System.Security.Cryptography;
using System;
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

            foreach (var row in userTable)
            {
                string storedUsername = row.username;
                string storedPasswordHash = row.password;

                if (storedUsername == username)
                {
                    userFound = true;

                    if (VerifyPassword(password, storedPasswordHash))
                    {
                        CheckRole(row, row.user_id);
                        return;
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
                MessageBox.Show("НЕ НАШЕЛ ЮЗЕРА!!!");
            }
        }

        public static bool VerifyPassword(string enteredPassword, string storedHash)
        {
            byte[] hashBytes = Convert.FromBase64String(storedHash);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, salt, 10000);
            byte[] enteredHash = pbkdf2.GetBytes(20);

            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != enteredHash[i])
                    return false;
            }
            return true;
        }

        private void CheckRole(FoodDeliveryDBDataSet.UsersRow row, int userID)
        {
            if (row.role_id == 1)
            {
                MessageBox.Show("Admin");
                AdminWindow admin = new AdminWindow();
                admin.Show();

            }
            else if (row.role_id == 2)
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

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
