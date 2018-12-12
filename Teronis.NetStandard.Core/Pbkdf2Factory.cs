using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Teronis.NetStandard
{
    public static class Pbkdf2Factory
    {
        public static Pbkdf2Hash GenerateHash(string password) {
            var container = new Pbkdf2Hash();
            GenerateHash(container, password);
            return container;
        }

        public static void GenerateHash(IPbkdf2Hash container, string password)
        {
            int iterations = 40000, saltBytes = 128 / 8, hashBytes = 256 / 8;
            byte[] salt, hash;

            // 128 bit salt
            salt = new byte[saltBytes];

            using (var rng = RandomNumberGenerator.Create()) {
                rng.GetBytes(salt);
            }

            hash = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA256, iterations, hashBytes);

            container.Hash = Convert.ToBase64String(hash);
            container.Salt = Convert.ToBase64String(salt);
            container.Interations = iterations;
    }
}
}
