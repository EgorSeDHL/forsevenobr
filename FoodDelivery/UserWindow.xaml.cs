using FoodDelivery.FoodDeliveryDBDataSetTableAdapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FoodDelivery
{
    /// <summary>
    /// Логика взаимодействия для UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {

        UsersTableAdapter users = new UsersTableAdapter();
        RolesTableAdapter roles = new RolesTableAdapter();
        Menu_ItemsTableAdapter menu = new Menu_ItemsTableAdapter();
        OrdersTableAdapter orders = new OrdersTableAdapter();
        PaymentsTableAdapter payments = new PaymentsTableAdapter();
        Order_ItemsTableAdapter order_items = new Order_ItemsTableAdapter();
        RestaurantsTableAdapter restaurants = new RestaurantsTableAdapter();
        ReviewsTableAdapter reviews = new ReviewsTableAdapter();
        public int user_ID;
        public decimal totalPrice;
        int selectedRestaurantId;
        List<int> itemsIDs = new List<int>();
        public int lastSelectedRestaurantId;
        public int currentSelectedRestaurantId;
        public UserWindow(int userId)
        {
            InitializeComponent();

            var restaurantsObject = restaurants.GetData();
            List<Restaurant> restaurantsList = new List<Restaurant>();
            user_ID = userId;
            var UsersGetData = users.GetData();
            string username = "NoName";
            foreach (var user in UsersGetData)
            {
                if (user.user_id == user_ID)
                {
                    username = user.username;
                }

            }
            HelloUserBX.Text = "Привет " + username + "!";

            foreach (var item in restaurantsObject)
            {
                restaurantsList.Add(new Restaurant { Id = item.restaurant_id, Name = item.name });
            }
            RestaurantComboBox.ItemsSource = restaurantsList;
            RestaurantComboBox.DisplayMemberPath = "Name";
        }

        private void RestaurantComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            // Получаем выбранный ресторан
            Restaurant selectedRestaurant = RestaurantComboBox.SelectedItem as Restaurant;
            currentSelectedRestaurantId = selectedRestaurant.Id;
            if (selectedRestaurant != null)
            {
                selectedRestaurantId = selectedRestaurant.Id;

                // Теперь можно использовать selectedRestaurantId для фильтрации данных

                var review = reviews.GetData();
                List<string> restaurantReviews = new List<string>();
                foreach (var item in review)
                {
                    if (item.restaurant_id == selectedRestaurantId)
                    {
                        restaurantReviews.Add(item.comment);
                    }
                }
                additionalInformationTBlock.ItemsSource = restaurantReviews;


                var menuObject = menu.GetData();
                List<MenuItem> menuRestaurantsList = new List<MenuItem>();
                foreach (var item in menuObject)
                {
                    if (item.restaurant_id == selectedRestaurantId)
                    {
                        menuRestaurantsList.Add(new MenuItem(item.item_id, item.restaurant_id, item.name, item.description, item.price, item.is_available, item.weight));
                    }
                }
                ProductListBox.ItemsSource = menuRestaurantsList;
                ProductListBox.DisplayMemberPath = "Name";

            }
        }
        
        private void SubmitOrder_Click(object sender, RoutedEventArgs e)
        {
            orders.InsertQuery(user_ID, null, DateTime.Now, totalPrice, "Pending", AddressTextBox.Text);
            var ordersGetData = orders.GetData();
            var lastOrderId = ordersGetData.Last().order_id;  
            CountClass countClass = new CountClass();
            Доработать 
            //foreach (var item in itemsIDs)
            //{
            //    order_items.InsertQuery(lastOrderId, item, countClass.quantity);

            //}
        }

        private void ProductListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            totalPrice = 0;
            var selectedItems = ProductListBox.SelectedItems;
            amountItemWibdow amountItemWibdow = new amountItemWibdow();
            amountItemWibdow.Show();
            itemsIDs.Clear();

            foreach (var item in selectedItems)
            {
                var selectedItem = item as MenuItem;

                if (selectedItem != null)
                {
                    itemsIDs.Add(selectedItem.ItemId);

                    totalPrice += selectedItem.Price;
                }
            }
            string ids = "";
            foreach (int i in itemsIDs)
            {
                ids += i;
            }
            MessageBox.Show(ids);
        }
    }
}
