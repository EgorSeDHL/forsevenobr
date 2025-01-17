﻿using FoodDelivery.FoodDeliveryDBDataSetTableAdapters;
using System;
using System.Windows;
using System.Windows.Controls;

namespace FoodDelivery.Admin
{
    /// <summary>
    /// Логика взаимодействия для AdminEditCourierWindow.xaml
    /// </summary>
    public partial class AdminEditCourierWindow : Window
    {
        UsersTableAdapter users = new UsersTableAdapter();
        String password;
        int userID;
        public AdminEditCourierWindow(int id)
        {
            userID = id;
            var allUsers = users.GetData();
            InitializeComponent();
            RoleCB.Items.Add("Администратор");
            RoleCB.Items.Add("Пользователь");
            RoleCB.Items.Add("Курьер");
            MessageBox.Show(id.ToString());
            foreach (var user in allUsers)
            {
                if (user.user_id == id)
                {
                    courierNameTB.Text = user.username;
                    courierEmailTB.Text = user.email;
                    courierPhoneTB.Text = user.phone.ToString();
                    courierAddressTB.Text = user.address;
                    RoleCB.SelectedIndex = user.role_id - 1;
                    password = user.password;
                }
            }
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void ChangeBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                users.UpdateQuery(courierNameTB.Text, password, courierEmailTB.Text, courierPhoneTB.Text, courierAddressTB.Text, RoleCB.SelectedIndex + 1, userID);
                MessageBox.Show("Успешное изменение");
                this.Close();
                AdminWindow adminWindow = new AdminWindow();
                adminWindow.Show();
            }
            catch
            {
                MessageBox.Show("Ошибкa " + e.ToString());
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            users.DeleteQuery(userID);
        }

        private void RoleCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            AdminWindow adminWindow = new AdminWindow();
            adminWindow.Show();
            this.Close();
        }
    }
}
