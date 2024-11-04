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
