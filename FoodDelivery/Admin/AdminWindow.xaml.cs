using FoodDelivery.FoodDeliveryDBDataSetTableAdapters;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace FoodDelivery.Admin
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        UsersTableAdapter users = new UsersTableAdapter();
        List<MyUser> usersList = new List<MyUser>();
        public AdminWindow()
        {
            InitializeComponent();
            var allUsers = users.GetData();
            foreach (var user in allUsers)
            {
                if (user.user_id != 1)
                    usersList.Add(new MyUser(user.user_id, user.username));
            }
            CourierLB.ItemsSource = usersList;
        }

        private void CourierLB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CourierLB.SelectedItem is MyUser selectedUser)
            {
                int userId = selectedUser.getUserID();

                AdminEditCourierWindow adminEditCourier = new AdminEditCourierWindow(userId);
                adminEditCourier.Show();
                this.Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AdminAddRestaurant adminAddRestaurant = new AdminAddRestaurant();
            adminAddRestaurant.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Class1 class1 = new Class1();
            class1.BackupDatabase("workstation id=FoodDeliveryDB.mssql.somee.com;packet size=4096;user id=highlighttt_SQLLogin_1;pwd=b15at2v9g8;data source=FoodDeliveryDB.mssql.somee.com;persist security info=False;initial catalog=FoodDeliveryDB;TrustServerCertificate=True", "C:\\Users\\egork\\Desktop\\4 курс");
        }
    }
}
