using FoodDelivery.FoodDeliveryDBDataSetTableAdapters;
using System.Windows;

namespace FoodDelivery.Admin
{
    /// <summary>
    /// Логика взаимодействия для AdminAddRestaurant.xaml
    /// </summary>
    public partial class AdminAddRestaurant : Window
    {
        RestaurantsTableAdapter restaurantsTableAdapter = new RestaurantsTableAdapter();

        public AdminAddRestaurant()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                restaurantsTableAdapter.InsertQuery(restaurantNameTBX.Text, addressTBX.Text, phoneTBX.Text, 5);
                AddRestaurantItemsWindow addRestaurantItemsWindow = new AddRestaurantItemsWindow();
                addRestaurantItemsWindow.Show();
                this.Close();
            }
            catch
            {
                MessageBox.Show("Ошибка: " + e);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
