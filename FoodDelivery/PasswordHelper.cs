using System;
using System.Security.Cryptography;

namespace FoodDelivery
{

    public class PasswordHelper
    {
        public static string HashPassword(string password, out string salt)
        {
            byte[] saltBytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 10000))
            {
                byte[] hash = pbkdf2.GetBytes(20); 
                byte[] hashBytes = new byte[36];
                Array.Copy(saltBytes, 0, hashBytes, 0, 16);
                Array.Copy(hash, 0, hashBytes, 16, 20);

                salt = Convert.ToBase64String(saltBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

        public static bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
        {
            byte[] saltBytes = Convert.FromBase64String(storedSalt);

            using (var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, saltBytes, 10000))
            {
                byte[] enteredHash = pbkdf2.GetBytes(20);
                byte[] storedHashBytes = Convert.FromBase64String(storedHash);

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
