using FoodDelivery.FoodDeliveryDBDataSetTableAdapters;
using System;
using System.Linq;
using System.Windows;

namespace FoodDelivery.Admin
{
    /// <summary>
    /// Логика взаимодействия для AddRestaurantItemsWindow.xaml
    /// </summary>
    public partial class AddRestaurantItemsWindow : Window
    {
        RestaurantsTableAdapter adapter = new RestaurantsTableAdapter();
        Menu_ItemsTableAdapter menu = new Menu_ItemsTableAdapter();
        int lastRestaurantId;

        public AddRestaurantItemsWindow()
        {
            InitializeComponent();

            var allRestaurants = adapter.GetData();
            if (allRestaurants.Count > 0)
            {
                lastRestaurantId = allRestaurants[allRestaurants.Count - 1].restaurant_id;
            }

            UpdateMenuItems();
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void addItem_Click(object sender, RoutedEventArgs e)
        {
            // Добавляем новый элемент меню
            menu.InsertQuery(lastRestaurantId, nameTBX.Text, discriptionTBX.Text, Convert.ToDecimal(priceTBX.Text), true, Convert.ToInt32(weightTBX.Text));

            // Обновляем rtb после добавления нового элемента
            UpdateMenuItems();
        }

        // Метод для обновления элементов в rtb
        private void UpdateMenuItems()
        {
            // Получаем все элементы меню и фильтруем по lastRestaurantId
            var allMenuItems = menu.GetData();
            var filteredMenuItems = allMenuItems.Where(item => item.restaurant_id == lastRestaurantId);

            // Устанавливаем отфильтрованные элементы меню как источник данных для rtb
            rtb.ItemsSource = null; // Сбрасываем ItemsSource, если оно было установлено ранее
            rtb.ItemsSource = filteredMenuItems; // Назначаем новые данные        }

        }
    }
}
