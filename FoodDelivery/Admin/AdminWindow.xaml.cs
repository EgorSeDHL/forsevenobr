using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using System.Data.SqlClient;
using System.Collections.Generic;
using FoodDelivery.FoodDeliveryDBDataSetTableAdapters;

namespace FoodDelivery.Admin
{
    public partial class AdminWindow : Window
    {
        UsersTableAdapter users = new UsersTableAdapter();
        List<MyUser> usersList = new List<MyUser>();
        // Конструктор
        public AdminWindow()
        {
            InitializeComponent();
            LoadUsers();
        }

        private void LoadUsers()
        {
            var allUsers = users.GetData();
            foreach (var user in allUsers)
            {
                if (user.user_id != 1)
                    usersList.Add(new MyUser(user.user_id, user.username));
            }
            CourierLB.ItemsSource = usersList;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SaveDatabaseToCSV();
            SaveDatabaseToSQL();
            MessageBox.Show("Данные успешно сохранены!");
        }

        // Экспорт всей базы данных в CSV
        private void SaveDatabaseToCSV()
        {
            var connectionString = "workstation id=FoodDeliveryDB.mssql.somee.com;packet size=4096;user id=highlighttt_SQLLogin_1;pwd=b15at2v9g8;data source=FoodDeliveryDB.mssql.somee.com;persist security info=False;initial catalog=FoodDeliveryDB;TrustServerCertificate=True"; // Укажите строку подключения к вашей базе данных
            var tables = new[] { "Users", "Orders", "Restaurants", "Menu_Items", "Payments", "Order_Items", "Reviews" }; // Список таблиц для экспорта

            foreach (var table in tables)
            {
                StringBuilder csvContent = new StringBuilder();
                var query = $"SELECT * FROM {table}";
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var command = new SqlCommand(query, connection);
                    var reader = command.ExecuteReader();

                    // Получаем имена столбцов
                    var columnNames = new List<string>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        columnNames.Add(reader.GetName(i));
                    }
                    csvContent.AppendLine(string.Join(",", columnNames));

                    // Получаем данные
                    while (reader.Read())
                    {
                        var row = new List<string>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row.Add(reader.GetValue(i).ToString());
                        }
                        csvContent.AppendLine(string.Join(",", row));
                    }
                }

                // Диалог сохранения файла
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "CSV file (*.csv)|*.csv",
                    FileName = $"{table}_Export.csv"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    File.WriteAllText(saveFileDialog.FileName, csvContent.ToString());
                }
            }
        }

        // Экспорт всей базы данных в SQL
        private void SaveDatabaseToSQL()
        {
            var connectionString = "workstation id=FoodDeliveryDB.mssql.somee.com;packet size=4096;user id=highlighttt_SQLLogin_1;pwd=b15at2v9g8;data source=FoodDeliveryDB.mssql.somee.com;persist security info=False;initial catalog=FoodDeliveryDB;TrustServerCertificate=True"; // Укажите строку подключения к вашей базе данных
            var tables = new[] { "Users", "Orders", "Restaurants", "Menu_Items", "Payments", "Order_Items", "Reviews"}; // Список таблиц для экспорта
            StringBuilder sqlContent = new StringBuilder();

            foreach (var table in tables)
            {
                var query = $"SELECT * FROM {table}";
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var command = new SqlCommand(query, connection);
                    var reader = command.ExecuteReader();

                    // Генерация SQL-запросов
                    sqlContent.AppendLine($"-- Export for {table}");
                    while (reader.Read())
                    {
                        var columns = new List<string>();
                        var values = new List<string>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            columns.Add(reader.GetName(i));
                            values.Add($"'{reader.GetValue(i).ToString()}'");
                        }
                        sqlContent.AppendLine($"INSERT INTO {table} ({string.Join(",", columns)}) VALUES ({string.Join(",", values)});");
                    }
                }
            }

            // Диалог сохранения файла
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "SQL file (*.sql)|*.sql",
                FileName = "DatabaseExport.sql"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, sqlContent.ToString());
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AdminAddRestaurant adminAddRestaurant = new AdminAddRestaurant();
            adminAddRestaurant.Show();
        }
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void CourierLB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CourierLB.SelectedItem is MyUser selectedUser)
            {
                int userId = selectedUser.getUserID();

                AdminEditCourierWindow adminEditCourier = new AdminEditCourierWindow(userId);
                adminEditCourier.Show();
                this.Close();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
