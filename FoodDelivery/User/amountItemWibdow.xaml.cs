using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FoodDelivery
{
    /// <summary>
    /// Логика взаимодействия для amountItemWibdow.xaml
    /// </summary>
    public partial class amountItemWibdow : Window
    {
        public int SelectedQuantity { get; private set; }

        public amountItemWibdow()
        {
            InitializeComponent();
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Проверяем, является ли вводимое значение числом
            if (!int.TryParse(e.Text, out int input))
            {
                e.Handled = true; // Блокируем ввод, если это не число
                return;
            }

            // Получаем текущее значение и добавляем новый ввод
            var textBox = sender as TextBox;
            string newText = textBox.Text.Insert(textBox.SelectionStart, e.Text);

            // Проверяем, что введенное значение в пределах диапазона
            if (int.TryParse(newText, out int newValue))
            {
                if (newValue < 1 || newValue > 10)
                {
                    e.Handled = true; // Блокируем ввод, если выходит за пределы
                }
            }
            else
            {
                e.Handled = true; // Блокируем ввод, если результат не число
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Устанавливаем количество и закрываем окно
            SelectedQuantity = Convert.ToInt16(countBX.Text);
            this.DialogResult = true; // Указываем, что окно завершено успешно
            this.Close();
        }
    }

}
