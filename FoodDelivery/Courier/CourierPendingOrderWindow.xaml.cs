using FoodDelivery.FoodDeliveryDBDataSetTableAdapters;
using System;
using System.Windows;

namespace FoodDelivery.Courier
{
    /// <summary>
    /// Логика взаимодействия для CourierPendingOrderWindow.xaml
    /// </summary>
    public partial class CourierPendingOrderWindow : Window
    {
        int orderID;
        int courierID;
        OrdersTableAdapter orders = new OrdersTableAdapter();
        public CourierPendingOrderWindow(int orderNumber, String Address, int courierid)
        {
            courierID = courierid;
            orderID = orderNumber;
            InitializeComponent();
            order_numberTB.Text += orderNumber.ToString();
            addressTB.Text += Address;
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            orders.UpdateQuery(null, "Canceled", orderID);
            CourierWindow courierWindow = new CourierWindow(courierID);
            courierWindow.Show();
            this.Close();
        }

        private void GiveOrderBtn_Click(object sender, RoutedEventArgs e)
        {
            orders.UpdateQuery(courierID, "Delivered", orderID);
            CourierWindow courierWindow = new CourierWindow(courierID);
            courierWindow.Show();
            this.Close();

        }
    }
}
