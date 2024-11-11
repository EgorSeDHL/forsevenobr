using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FoodDelivery
{

    public class PasswordHelper
    {
        // Метод для генерации хэша пароля с солью
        public static string HashPassword(string password, out string salt)
        {
            // Генерация случайной соли
            byte[] saltBytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }

            // Хэширование пароля с солью
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 10000))
            {
                byte[] hash = pbkdf2.GetBytes(20); // Получение 20 байт хэша

                // Сохранение соли и хэша в единую строку
                byte[] hashBytes = new byte[36];
                Array.Copy(saltBytes, 0, hashBytes, 0, 16);
                Array.Copy(hash, 0, hashBytes, 16, 20);

                // Конвертация результата в строку для хранения
                salt = Convert.ToBase64String(saltBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

        // Метод для проверки пароля
        public static bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
        {
            byte[] saltBytes = Convert.FromBase64String(storedSalt);

            // Хэширование введенного пароля с использованием соли
            using (var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, saltBytes, 10000))
            {
                byte[] enteredHash = pbkdf2.GetBytes(20);
                byte[] storedHashBytes = Convert.FromBase64String(storedHash);

                // Сравнение хэша введенного пароля и сохраненного хэша
                for (int i = 0; i < 20; i++)
                {
                    if (enteredHash[i] != storedHashBytes[i + 16])
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }

}
