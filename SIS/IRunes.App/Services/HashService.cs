using System;
using System.Security.Cryptography;
using System.Text;
using IRunes.App.Services.Interfaces;

namespace IRunes.App.Services
{
    public class HashService : IHashService
    {
        private const string Salt = "IRunes_Some_Salt";

        public string ComputeSha256Hash(string password)
        {
            password = password + Salt;

            using (var sha256Hash = SHA256.Create())
            {
                var hashedBytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

                return hash;
            }
        }
    }
}
