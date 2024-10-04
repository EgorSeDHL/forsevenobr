using FoodDelivery.foodDeliveryDBDataSet1TableAdapters;
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

            // Получаем все данные пользователей
            var userTable = users.GetData();

            bool userFound = false;

            // Ищем пользователя и проверяем пароль
            foreach (var row in userTable)
            {
                // Предположим, что в строке есть поля Username и Password
                string storedUsername = row.username; // замените на реальное имя поля
                string storedPassword = row.password; // замените на реальное имя поля

                if (storedUsername == username)
                {
                    userFound = true;

                    if (storedPassword == password)
                    {
                        MessageBox.Show("НАШЕЕЕЛ!!!");
                        return; // Выход из метода при успешном входе
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
    }
}
