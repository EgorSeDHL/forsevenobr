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
using FoodDelivery.foodDeliveryDBDataSetTableAdapters;

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
        public UserWindow(int userId)
        {
            InitializeComponent();

            var restaurantsObject = restaurants.GetData();
            List<Restaurant> restaurantsList = new List<Restaurant>();
            user_ID = userId;
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
                List<string> menuRestaurantsList = new List<string>();
                foreach (var item in menuObject)
                {
                    if (item.restaurant_id == selectedRestaurantId)
                    {
                        menuRestaurantsList.Add(item.name);
                    }
                }
                ProductListBox.ItemsSource = menuRestaurantsList;
                
            }
        }

        private void SubmitOrder_Click(object sender, RoutedEventArgs e)
        {
            order_items.InsertQuery(selectedRestaurantId,);
            orders.InsertQuery(user_ID, DateTime.Now, totalPrice, "В ожидании", 1, AddressTextBox.Text);
        }

        private void ProductListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItems = ProductListBox.SelectedItems;

            // Считаем сумму цен выбранных элементов

            foreach (var item in selectedItems)
            {
                if (item is Product product) // Убедитесь, что это ваш класс
                {
                    totalPrice += product.Price; // Добавляем цену к общей сумме
                }
            }

        }
    }
}
