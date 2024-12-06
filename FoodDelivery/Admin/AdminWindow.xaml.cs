using FoodDelivery.FoodDeliveryDBDataSetTableAdapters;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;

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
            RestoreDatabase();
            MessageBox.Show("Данные успешно сохранены!");
        }

        // Экспорт всей базы данных в CSV
        private void SaveDatabaseToCSV()
        {
            var connectionString = "workstation id=FoodDeliveryDB.mssql.somee.com;packet size=4096;user id=highlighttt_SQLLogin_1;pwd=b15at2v9g8;data source=FoodDeliveryDB.mssql.somee.com;persist security info=False;initial catalog=FoodDeliveryDB;TrustServerCertificate=True"; // Укажите строку подключения к вашей базе данных
            var tables = new[] { "Roles", "Users", "Orders", "Restaurants", "Menu_Items", "Payments", "Order_Items", "Reviews" }; // Список таблиц для экспорта

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
            var tables = new[] { "Roles", "Users", "Orders", "Restaurants", "Menu_Items", "Payments", "Order_Items", "Reviews" }; // Список таблиц для экспорта
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
                            var value = reader.GetValue(i);
                            if (value is string || value is DateTime)
                            {
                                values.Add($"'{value}'");
                            }
                            else if (value is DBNull)
                            {
                                values.Add("NULL");
                            }
                            else
                            {
                                values.Add(value.ToString());
                            }
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

        private void RestoreDatabase()
        {
            string selectedFileName = $"DB{DateTime.Now.Ticks}.bak";

            string backupDirectory = @"C:\Backup\";

            string backupFilePath = System.IO.Path.Combine(backupDirectory, System.IO.Path.GetFileName(selectedFileName));

            try
            {
                string backupCommand = $"BACKUP DATABASE FoodDeliveryDB TO DISK = '{backupFilePath}';";
                string connectionString = "Data Source=EGORLAPTOP\\MSSQLSERVER01;Initial Catalog=FoodDeliveryBD;Integrated Security=True;TrustServerCertificate=True;";
                using (var connection = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    connection.Open();

                    using (var command = new System.Data.SqlClient.SqlCommand(backupCommand, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                MessageBox.Show($"Backup успешно сохранен в путь восстановления");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
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
        private void LoadDatabaseButton_Click(object sender, RoutedEventArgs e)
        {
            AdminDiagramWindow adminDiagramWindow = new AdminDiagramWindow();
            adminDiagramWindow.Show();
        }
        public void LoadDatabaseFromSQL(string connectionString, string sqlFilePath)
        {
            try
            {
                // Читаем содержимое SQL-файла
                string sqlCommands = File.ReadAllText(sqlFilePath);

                // Подключаемся к базе данных
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Разделяем команды на основе ";"
                    var commands = sqlCommands.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var command in commands)
                    {
                        if (!string.IsNullOrWhiteSpace(command))
                        {
                            // Проверяем, если команда содержит вставку в таблицу "Roles"
                            if (command.Contains("INSERT INTO Roles"))
                            {
                                using (SqlCommand enableIdentityInsertRoles = new SqlCommand("SET IDENTITY_INSERT Roles ON;", connection))
                                {
                                    enableIdentityInsertRoles.ExecuteNonQuery();
                                }

                                using (SqlCommand sqlCommand = new SqlCommand(command, connection))
                                {
                                    sqlCommand.ExecuteNonQuery();
                                }

                                using (SqlCommand disableIdentityInsertRoles = new SqlCommand("SET IDENTITY_INSERT Roles OFF;", connection))
                                {
                                    disableIdentityInsertRoles.ExecuteNonQuery();
                                }
                            }
                            // Проверяем, если команда содержит вставку в таблицу "Users"
                            else if (command.Contains("INSERT INTO Users"))
                            {
                                using (SqlCommand enableIdentityInsertUsers = new SqlCommand("SET IDENTITY_INSERT Users ON;", connection))
                                {
                                    enableIdentityInsertUsers.ExecuteNonQuery();
                                }

                                using (SqlCommand sqlCommand = new SqlCommand(command, connection))
                                {
                                    sqlCommand.ExecuteNonQuery();
                                }

                                using (SqlCommand disableIdentityInsertUsers = new SqlCommand("SET IDENTITY_INSERT Users OFF;", connection))
                                {
                                    disableIdentityInsertUsers.ExecuteNonQuery();
                                }
                            }
                            // Проверяем, если команда содержит вставку в таблицу "Orders"
                            else if (command.Contains("INSERT INTO Orders"))
                            {
                                using (SqlCommand enableIdentityInsertOrders = new SqlCommand("SET IDENTITY_INSERT Orders ON;", connection))
                                {
                                    enableIdentityInsertOrders.ExecuteNonQuery();
                                }

                                using (SqlCommand sqlCommand = new SqlCommand(command, connection))
                                {
                                    sqlCommand.ExecuteNonQuery();
                                }

                                using (SqlCommand disableIdentityInsertOrders = new SqlCommand("SET IDENTITY_INSERT Orders OFF;", connection))
                                {
                                    disableIdentityInsertOrders.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                using (SqlCommand sqlCommand = new SqlCommand(command, connection))
                                {
                                    sqlCommand.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }

                MessageBox.Show("База данных успешно загружена из выбранного SQL-файла!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке базы данных: {ex.Message}");
            }
        }




        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }
    }
}
