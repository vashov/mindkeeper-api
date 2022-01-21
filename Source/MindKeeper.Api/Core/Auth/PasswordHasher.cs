using System;
using System.Security.Cryptography;

namespace MindKeeper.Api.Core.Auth
{
    public class PasswordHasher
    {
        public string CreateHash(string password)
        {
            byte[] salt = CreateSalt();
            byte[] passwordHash = GetPasswordHash(password, salt);

            byte[] combinedHash = Combine(passwordHash, salt);

            return Convert.ToBase64String(combinedHash);
        }

        public bool IsPasswordsEquals(string enteredPassword, string savedPasswordHash)
        {
            byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);

            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);

            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                    return false;
            }
            return true;
        }

        private byte[] CreateSalt()
        {
            using var cryptoService = new RNGCryptoServiceProvider();
            byte[] salt = new byte[16];
            cryptoService.GetBytes(salt);
            return salt;
        }

        private byte[] GetPasswordHash(string password, byte[] salt)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);
            return hash;
        }

        private byte[] Combine(byte[] hash, byte[] salt)
        {
            byte[] combinedHash = new byte[36];
            Array.Copy(salt, 0, combinedHash, 0, 16);
            Array.Copy(hash, 0, combinedHash, 16, 20);
            return combinedHash;
        }

    }
}
