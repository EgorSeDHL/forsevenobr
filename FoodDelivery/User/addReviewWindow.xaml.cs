using FoodDelivery.FoodDeliveryDBDataSetTableAdapters;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FoodDelivery.User
{
    /// <summary>
    /// Логика взаимодействия для addReviewWindow.xaml
    /// </summary>
    public partial class addReviewWindow : Window
    {
        ReviewsTableAdapter reviewsTableAdapter = new ReviewsTableAdapter();
        private int? restaurant_id;
        private int? user_id;
        private int rating;
        private void Star_Click(object sender, RoutedEventArgs e)
        {
            // Определяем рейтинг на основе кнопки, на которую нажали
            Button clickedButton = sender as Button;
            rating = int.Parse(clickedButton.Tag.ToString());

            // Обновляем внешний вид звездочек
            UpdateStarAppearance();

            // Отображаем текущий рейтинг
        }

        private void UpdateStarAppearance()
        {
            // Обновляем цвет каждой звезды в зависимости от текущего рейтинга
            foreach (Button starButton in RatingPanel.Children)
            {
                int starValue = int.Parse(starButton.Tag.ToString());
                if (starValue <= rating)
                {
                    // Выбранные звезды — желтый цвет
                    starButton.Foreground = new SolidColorBrush(Colors.Gold);
                }
                else
                {
                    // Не выбранные звезды — серый цвет
                    starButton.Foreground = new SolidColorBrush(Colors.Gray);
                }
            }
        }
        public addReviewWindow(int restaurantID, int userID)
        {
            restaurant_id = restaurantID;
            user_id = userID;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            reviewsTableAdapter.InsertQuery(restaurant_id, user_id, rating, ReviewTB.Text, DateTime.Now);
            this.Close();
        }
    }
}
