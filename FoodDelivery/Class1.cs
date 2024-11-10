using System;
using System.Data.SqlClient;
using System.Windows;

namespace FoodDelivery
{
    internal class Class1
    {

        public void BackupDatabase(string connectionString, string backupPath)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string backupQuery = $"BACKUP DATABASE [FoodDeliveryDB] TO DISK = '{backupPath}'";

                    using (SqlCommand command = new SqlCommand(backupQuery, connection))
                    {
                        command.ExecuteNonQuery();
                        MessageBox.Show("Бэкап успешно создан!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании бэкапа: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
