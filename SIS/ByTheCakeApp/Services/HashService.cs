using System;
using System.Security.Cryptography;
using System.Text;
using ByTheCake.App.Services.Interfaces;

namespace ByTheCake.App.Services
{
    public class HashService : IHashService
    {
        private const string Salt = "sis_cake_app_some_salt";

        public string ComputeSha256Hash(string rawData)
        {
            rawData = rawData + Salt;

            using (var sha256Hash = SHA256.Create())
            {
                var hashedBytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

                return hash;
            }
        }
    }
}
