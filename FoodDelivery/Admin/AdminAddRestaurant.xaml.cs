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
                    MessageBox.Show("Успешно");
                    this.Close();

            }
            catch
            {
                MessageBox.Show("Ошибка: " + e.ToString());
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
