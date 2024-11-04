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

namespace FoodDelivery.Courier
{
    /// <summary>
    /// Логика взаимодействия для CourierDetailWindow.xaml
    /// </summary>
    public partial class CourierDetailWindow : Window
    {
        OrdersTableAdapter orders = new OrdersTableAdapter();
        Order_ItemsTableAdapter orderItems = new Order_ItemsTableAdapter();
        Menu_ItemsTableAdapter menuItems = new Menu_ItemsTableAdapter();
        UsersTableAdapter users = new UsersTableAdapter();
        String username;
        String address;
        List<MenuItem> ListItems = new List<MenuItem>();
        int OrderID;
        int courierID;

        public CourierDetailWindow(int orderID, int CourierID)
        {
            courierID = CourierID;
            OrderID = orderID;
            InitializeComponent();
            var allOrders = orders.GetData();
            var allUsers = users.GetData();
            var allItems = orderItems.GetData();
            var allProducts = menuItems.GetData();
            foreach (var item in allOrders)
                {                   
                    if (item.order_id == orderID)
                {
                    foreach (var orderitems in allItems) 
                    { 
                        if (orderitems.order_id == orderID)
                        {
                            foreach(var products in allProducts)
                            {
                                if(products.item_id == orderitems.item_id)
                                {
                                    ListItems.Add(new MenuItem(products.item_id, products.restaurant_id, products.name, products.description, products.price, products.is_available, products.weight));
                                }
                            }
                        }
                    }
                    address = item.order_address;
                    foreach(var user in allUsers)
                    {
                        if(item.user_id == user.user_id)
                        {
                            username = user.username;
                        }
                    }
                }
            }
           
            userTB.Text = $"Имя заказчика: {username}";
            addressTB.Text = $"Адрес заказчика: {address}";
            productsLB.ItemsSource = ListItems;
            productsLB.DisplayMemberPath = "Name";
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            CourierWindow courierWindow = new CourierWindow(courierID);
            courierWindow.Show();
            this.Close();
        }

        private void takeOrderBtn_Click(object sender, RoutedEventArgs e)
        {
            orders.UpdateQuery(courierID, "Pending", OrderID);
            CourierPendingOrderWindow courierPendingOrderWindow = new CourierPendingOrderWindow(OrderID,address, courierID);
            courierPendingOrderWindow.Show();
            this.Close();
        }
    }
}
