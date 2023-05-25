using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WebRecipesApi.BusinessLogic
{
    public class PasswordHasher
    {
        private static RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
        private static readonly int SaltSize = 16;
        private static readonly int HashSize = 20;
        private static readonly int Iterations = 10000;


        public static string HashPassword(string password)
        {
            byte[] salt;
            rng.GetBytes(salt = new byte[SaltSize]);

            // Create a key using the password and salt
            var key = new Rfc2898DeriveBytes(password, salt, Iterations);

            // Generate the hash
            var hash = key.GetBytes(HashSize);

            // Combine the salt and hash into a single byte array
            var hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            // Convert the byte array to a base64 string
            var base64Hash = Convert.ToBase64String(hashBytes);

            return base64Hash;
        }


        public static bool VerifyPassword(string password, string base64Hash)
        {
            var hashBytes = Convert.FromBase64String(base64Hash);

            // Extract the salt from the hash
            var salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            // Create a key using the password and salt
            var key = new Rfc2898DeriveBytes(password, salt, Iterations);

            // Generate the hash using the key
            byte[] hash = key.GetBytes(HashSize);

            // Compare each byte of the stored hash with the computed hash
            for (var i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + SaltSize] != hash[i])
                    return false;
            }

            return true;
        }
    }
}
