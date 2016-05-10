using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace UI.Logic
{
    public class SaltedHash
    {
        public string Hash { get; private set; }
        public string Salt { get; private set; }

        public SaltedHash(string password)
        {
            var saltBytes = new byte[64];
            new Random().NextBytes(saltBytes);
            Salt = Convert.ToBase64String(saltBytes);
            var passwordAndSaltBytes = Concat(password, saltBytes);
            Hash = ComputeHash(passwordAndSaltBytes);
        }

        static string ComputeHash(byte[] bytes)
        {
            HashAlgorithmProvider hasher = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha512);

            IBuffer buffer = bytes.AsBuffer();
            IBuffer buffHash = hasher.HashData(buffer);

            String strHashBase64 = CryptographicBuffer.EncodeToBase64String(buffHash);
            return strHashBase64;
        }

        static byte[] Concat(string password, byte[] saltBytes)
        {
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            return passwordBytes.Concat(saltBytes).ToArray();
        }

        public static bool Verify(string salt, string hash, string password)
        {
            var saltBytes = Convert.FromBase64String(salt);
            var passwordAndSaltBytes = Concat(password, saltBytes);
            var hashAttempt = ComputeHash(passwordAndSaltBytes);
            return hash == hashAttempt;
        }
    }
}
