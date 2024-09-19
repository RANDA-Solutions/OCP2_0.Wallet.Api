using System;
using System.Security.Cryptography;
using System.Text;

namespace OpenCredentialPublisher.Shared.Utilities
{
    public class AccessCodeGenerator
    {
        public static string GenerateUniqueNumericCode(byte[] source)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(source);
            var result = new StringBuilder();
            foreach (byte b in bytes)
            {
                result.Append(b.ToString("x2")); // Convert to hexadecimal
            }

            var hashString = result.ToString();
            var numericHash =
                Math.Abs(BitConverter.ToInt64(Encoding.UTF8.GetBytes(hashString.Substring(0, 8)), 0));
            return (numericHash % 1000000).ToString("D6"); // Ensure it is 6 digits
        }
    }
}
