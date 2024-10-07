using FoodDelivery.foodDeliveryDBDataSetTableAdapters;
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

            var userTable = users.GetData();

            bool userFound = false;

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
        private void CheckRole(foodDeliveryDBDataSet.UsersRow row, int userID)
        {
            if (row.role_id == 1)
            {
                MessageBox.Show("Курьер");
            }
            if(row.role_id == 2)
            { 
                MessageBox.Show("Пользователь");
                UserWindow userWindow = new UserWindow(userID);
                userWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Admin");
            }

        }
    }
}
