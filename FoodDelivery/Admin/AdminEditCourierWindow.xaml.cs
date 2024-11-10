using FoodDelivery.FoodDeliveryDBDataSetTableAdapters;
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
            foreach(var user in allUsers)
            {
                if (user.user_id == id)
                {
                    courierNameTB.Text = user.username;
                    courierEmailTB.Text = user.email;
                    courierPhoneTB.Text = user.phone.ToString();
                    courierAddressTB.Text = user.address;
                    RoleCB.SelectedIndex = user.role_id-1;
                    password= user.password;
                }
            }
        }

        private void ChangeBtn_Click(object sender, RoutedEventArgs e)
        {
            users.UpdateQuery(courierNameTB.Text, password, courierEmailTB.Text, courierPhoneTB.Text, courierAddressTB.Text, RoleCB.SelectedIndex+1, userID);
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            users.DeleteQuery(userID);
        }

        private void RoleCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
