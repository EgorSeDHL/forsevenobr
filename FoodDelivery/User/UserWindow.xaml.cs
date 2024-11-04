using FoodDelivery.FoodDeliveryDBDataSetTableAdapters;
using FoodDelivery.User;
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
        public int restaurant_ID;
        public decimal totalPrice;
        int selectedRestaurantId;
        List<int> itemsIDs = new List<int>();
        public int lastSelectedRestaurantId;
        public int currentSelectedRestaurantId;
        List<OrderedItem> orderedItems = new List<OrderedItem>();
        String paymentMethod = "Pending";
        List<MenuItem> menuRestaurantsList = new List<MenuItem>();

        public UserWindow(int userId)
        {
            InitializeComponent();
            PaymentMethodComboBox.SelectedIndex = 0;
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
                restaurant_ID = item.restaurant_id;
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
            try
            {
                // Вставляем заказ и проверяем, что запрос выполнен успешно
                orders.InsertQuery(user_ID, null, DateTime.Now, totalPrice, "Pending", AddressTextBox.Text);
                var ordersGetData = orders.GetData();
                var lastOrderId = ordersGetData.Last().order_id;
                payments.Insert(lastOrderId, DateTime.Now, totalPrice, paymentMethod);
                // Проверка списка товаров перед вставкой
                if (orderedItems.Count == 0)
                {
                    MessageBox.Show("Список товаров пуст.");
                    return;
                }

                foreach (var orderedItem in orderedItems)
                {
                    // Вставляем каждый товар с количеством
                    order_items.InsertQuery(lastOrderId, orderedItem.ItemId, orderedItem.Quantity);

                    // Сообщение для отладки
                }

                MessageBox.Show("Заказ успешно добавлен!");
            }
            catch (Exception ex)
            {
                // Ловим исключение и выводим сообщение об ошибке
                MessageBox.Show($"Ошибка при добавлении заказа: {ex.Message}");
            }
        }


        // Добавляем список для корзины
        List<MenuItem> basketItemsList = new List<MenuItem>();

        private void ProductListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItems = ProductListBox.SelectedItems.Cast<MenuItem>().ToList(); // Получаем выбранные элементы
            itemsIDs.Clear();

            foreach (var selectedItem in selectedItems)
            {
                if (selectedItem != null)
                {
                    amountItemWibdow amountItemWibdow = new amountItemWibdow();
                    if (amountItemWibdow.ShowDialog() == true)
                    {
                        int selectedQuantity = amountItemWibdow.SelectedQuantity; // Получаем количество

                        // Добавляем товар с количеством в список заказов
                        orderedItems.Add(new OrderedItem(selectedItem.ItemId, selectedQuantity));

                        // Увеличиваем общую стоимость
                        totalPrice += selectedItem.Price * selectedQuantity;

                        // Перемещаем элемент в корзину
                        basketItemsList.Add(selectedItem);

                        // Удаляем элемент из списка продуктов
                        (ProductListBox.ItemsSource as List<MenuItem>).Remove(selectedItem);
                    }
                }
            }

            // Обновляем источники данных для ListBox
            ProductListBox.ItemsSource = null;
            ProductListBox.ItemsSource = menuRestaurantsList; // обновляем список продуктов
            ProductListBox.DisplayMemberPath = "Name";

            BasketListBox.ItemsSource = null;
            BasketListBox.ItemsSource = basketItemsList; // обновляем список корзины
            BasketListBox.DisplayMemberPath = "Name";

            // Обновляем отображение общей суммы
            totalSummTB.Text = totalPrice.ToString() + " Рублей";

            // Отображаем список товаров в корзине для отладки
            string ids = string.Join(", ", orderedItems.Select(o => $"ItemId: {o.ItemId}, Quantity: {o.Quantity}"));
            MessageBox.Show(ids);
        }



        private void additionalInformationTBlock_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void PaymentMethodComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PaymentMethodComboBox.SelectedIndex == 0)
            {
                paymentMethod = "Card";
            }
            else
            {
                paymentMethod = "Cash";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            addReviewWindow addReviewWindow = new addReviewWindow(restaurant_ID, user_ID);
            addReviewWindow.ShowDialog();
        }
    }
}
