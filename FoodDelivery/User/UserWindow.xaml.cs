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
        LoggingTableAdapter logging = new LoggingTableAdapter();
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

            // Получаем имя пользователя для приветствия
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

            // Добавляем фиктивный элемент "Все рестораны"
            restaurantsList.Add(new Restaurant { Id = -1, Name = "...Все рестораны..." });

            foreach (var item in restaurantsObject)
            {
                restaurantsList.Add(new Restaurant { Id = item.restaurant_id, Name = item.name });
            }
            RestaurantComboBox.ItemsSource = restaurantsList;
            RestaurantComboBox.DisplayMemberPath = "Name";
            RestaurantComboBox.SelectedIndex = 0; // Устанавливаем по умолчанию "Все рестораны"
        }


        private void RestaurantComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            menuRestaurantsList.Clear();

            // Получаем выбранный ресторан
            Restaurant selectedRestaurant = RestaurantComboBox.SelectedItem as Restaurant;
            if (selectedRestaurant != null)
            {
                if (selectedRestaurant.Id == -1)
                {
                    // Если выбран пункт "Все рестораны", сбрасываем выбор ресторана
                    selectedRestaurantId = -1;
                    restaurant_ID = -1;
                    currentSelectedRestaurantId = -1;
                }
                else
                {
                    selectedRestaurantId = selectedRestaurant.Id;
                    restaurant_ID = selectedRestaurant.Id;
                    currentSelectedRestaurantId = selectedRestaurant.Id;

                    // Заполняем отзывы для выбранного ресторана
                    var review = reviews.GetData();
                    var allusers = users.GetData();
                    List<string> restaurantReviews = new List<string>();
                    foreach (var item in review)
                    {
                        if (item.restaurant_id == selectedRestaurantId)
                        {
                            // Найти пользователя по item.user_id
                            var user = allusers.FirstOrDefault(u => u.user_id == item.user_id);
                            string username = user != null ? user.username : "Неизвестный пользователь";

                            restaurantReviews.Add($"👤 {username}: {item.comment}. Оценка {item.rating} ");
                        }
                    }
                    additionalInformationTBlock.ItemsSource = restaurantReviews;
                }

                // Обновляем список продуктов на основе выбранного ресторана
                UpdateProductList();
            }
        }


        private void UpdateProductList()
        {
            menuRestaurantsList.Clear();

            // Получаем все данные из меню
            var allProducts = menu.GetData();

            // Проверяем, выбран ли ресторан
            bool isRestaurantSelected = RestaurantComboBox.SelectedItem != null;

            // Проверяем, если поле поиска пустое
            if (string.IsNullOrWhiteSpace(searchTBX.Text))
            {
                // Если ресторан выбран, показываем только продукты этого ресторана
                if (isRestaurantSelected)
                {
                    foreach (var product in allProducts)
                    {
                        if (product.restaurant_id == selectedRestaurantId) // фильтруем по выбранному ресторану
                        {
                            menuRestaurantsList.Add(new MenuItem(product.item_id, product.restaurant_id, product.name, product.description, product.price, product.is_available, product.weight));
                        }
                    }
                }
                else
                {
                    // Если ресторан не выбран, добавляем все продукты
                    foreach (var product in allProducts)
                    {
                        menuRestaurantsList.Add(new MenuItem(product.item_id, product.restaurant_id, product.name, product.description, product.price, product.is_available, product.weight));
                    }
                }
            }
            else
            {
                // Если в поле поиска есть текст
                foreach (var product in allProducts)
                {
                    // Фильтруем продукты по введенному тексту и выбранному ресторану (если он выбран)
                    if (product.name.ToLower().Contains(searchTBX.Text.ToLower()) &&
                       (!isRestaurantSelected || product.restaurant_id == selectedRestaurantId))
                    {
                        menuRestaurantsList.Add(new MenuItem(product.item_id, product.restaurant_id, product.name, product.description, product.price, product.is_available, product.weight));
                    }
                }
            }

            // Обновляем источник данных для ProductListBox
            ProductListBox.ItemsSource = null;
            ProductListBox.ItemsSource = menuRestaurantsList;
        }


        private void SubmitOrder_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                // Вставляем заказ и проверяем, что запрос выполнен успешно
                orders.InsertQuery(user_ID, null, DateTime.Now, totalPrice, "Pending", AddressTextBox.Text);
                logging.InsertQuery($"Пользователь с id {user_ID} создал заказ с общей стомиостью {totalPrice}, на адрес {AddressTextBox.Text}");
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

            BasketListBox.ItemsSource = null;
            BasketListBox.ItemsSource = basketItemsList; // обновляем список корзины
            BasketListBox.DisplayMemberPath = "Name";

            // Обновляем отображение общей суммы
            totalSummTB.Text = totalPrice.ToString() + " $";

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
            addReviewWindow.Show();
            this.Close();
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void searchTBX_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Очищаем список продуктов перед новым поиском
            menuRestaurantsList.Clear();

            // Получаем все данные из меню
            var allProducts = menu.GetData();

            // Проверяем, если поле поиска пустое
            if (string.IsNullOrWhiteSpace(searchTBX.Text))
            {
                // Если поле поиска пустое, показываем все продукты, если выбран "Все рестораны"
                // или фильтруем по ресторану, если выбран конкретный ресторан
                foreach (var product in allProducts)
                {
                    if (selectedRestaurantId == -1 || product.restaurant_id == selectedRestaurantId)
                    {
                        menuRestaurantsList.Add(new MenuItem(
                            product.item_id,
                            product.restaurant_id,
                            product.name,
                            product.description,
                            product.price,
                            product.is_available,
                            product.weight
                        ));
                    }
                }
            }
            else
            {
                // Если в поле поиска есть текст, фильтруем продукты по тексту поиска и выбранному ресторану
                foreach (var product in allProducts)
                {
                    if (product.name.ToLower().Contains(searchTBX.Text.ToLower()) &&
                        (selectedRestaurantId == -1 || product.restaurant_id == selectedRestaurantId))
                    {
                        menuRestaurantsList.Add(new MenuItem(
                            product.item_id,
                            product.restaurant_id,
                            product.name,
                            product.description,
                            product.price,
                            product.is_available,
                            product.weight
                        ));
                    }
                }
            }

            // Обновляем источник данных для ProductListBox
            ProductListBox.ItemsSource = null;
            ProductListBox.ItemsSource = menuRestaurantsList;
            ProductListBox.DisplayMemberPath = "Name";
        }

        private void ProductListBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
