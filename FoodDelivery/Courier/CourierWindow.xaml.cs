using FoodDelivery.FoodDeliveryDBDataSetTableAdapters;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace FoodDelivery.Courier
{
    public class OrderDisplay
    {
        public int Id { get; set; }
        public string DisplayText { get; set; } // Адрес и вес для отображения
    }

    public partial class CourierWindow : Window
    {
        OrdersTableAdapter orders = new OrdersTableAdapter();
        Order_ItemsTableAdapter orderItems = new Order_ItemsTableAdapter();
        Menu_ItemsTableAdapter menuItems = new Menu_ItemsTableAdapter();
        int courierID;
        public CourierWindow(int id)
        {
            courierID = id;
            InitializeComponent();
            var ordersObject = orders.GetData();
            var orderItemsObject = orderItems.GetData();
            var menuItemsObject = menuItems.GetData();
            List<OrderDisplay> orderDisplayList = new List<OrderDisplay>();

            foreach (var item in ordersObject)
            {
                // Проверяем, что courier_id == null (или DBNull)
                if (item["courier_id"] == DBNull.Value)
                {
                    double weight = 0;

                    foreach (var item2 in orderItemsObject)
                    {
                        if (item.order_id == item2.order_id)
                        {
                            foreach (var item3 in menuItemsObject)
                            {
                                if (item2.item_id == item3.item_id)
                                {
                                    weight += item3.weight * item2.quantity;
                                }
                            }
                        }
                    }

                    // Добавляем данные с ID заказа и форматом "Адрес Вес"
                    orderDisplayList.Add(new OrderDisplay
                    {
                        Id = item.order_id,  // ID заказа из БД
                        DisplayText = $"Адрес: {item.order_address} Вес: {weight} кг"
                    });
                }
            }

            // Привязываем список к ListBox
            ItemsListBox.ItemsSource = orderDisplayList;
            ItemsListBox.DisplayMemberPath = "DisplayText";
        }


        private void ItemsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Получаем ID заказа из выбранного элемента
            if (ItemsListBox.SelectedItem is OrderDisplay selectedOrder)
            {
                CourierDetailWindow courierDetailWindow = new CourierDetailWindow(selectedOrder.Id, courierID);
                courierDetailWindow.Show();
                this.Close();
            }
        }
    }
}
