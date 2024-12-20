using FoodDelivery.FoodDeliveryDBDataSetTableAdapters;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Globalization;


namespace FoodDelivery.User
{
    /// <summary>
    /// Логика взаимодействия для addReviewWindow.xaml
    /// </summary>
    public partial class addReviewWindow : Window
    {
        ReviewsTableAdapter reviewsTableAdapter = new ReviewsTableAdapter();
        LoggingTableAdapter logging = new LoggingTableAdapter();
        private int? restaurant_id;
        private int? user_id;
        private int rating;

        private void Star_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;

            rating = int.Parse(clickedButton.Tag.ToString(), CultureInfo.InvariantCulture);

            UpdateStarAppearance();
        }
        private void UpdateStarAppearance()
        {
            // Получаем кнопки из RatingPanel и обновляем их цвет в зависимости от текущего рейтинга
            foreach (var child in RatingPanel.Children)
            {
                if (child is Button starButton)
                {
                    int starValue = int.Parse(starButton.Tag.ToString());
                    if (starValue <= rating)
                    {
                        // Выбранные звезды — золотой цвет
                        starButton.Content = "★";
                        starButton.Foreground = new SolidColorBrush(Colors.Gold);
                    }
                    else
                    {
                        // Не выбранные звезды — серый цвет
                        starButton.Content = "☆";
                        starButton.Foreground = new SolidColorBrush(Colors.Gray);
                    }
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
            logging.InsertQuery($"Пользователь с id {user_id} отзыв {ReviewTB.Text} для ресторана с id {restaurant_id}, c оценкой {rating}");

            UserWindow userWindow = new UserWindow(Convert.ToInt32(user_id));
            userWindow.Show();
            this.Close();
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            UserWindow userWindow = new UserWindow(int.Parse(user_id.ToString(), CultureInfo.InvariantCulture));
            userWindow.Show();
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
    }
}
