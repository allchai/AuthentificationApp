using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace HasherLib
{
    public class Hasher
    {
        public static string Hash(string input)
        {
            using (SHA256 shs256Hash = SHA256.Create())
            {
                byte[] sourceBytePassword = Encoding.UTF8.GetBytes(input);
                byte[] hash = shs256Hash.ComputeHash(sourceBytePassword);
                string uppercaseString = BitConverter.ToString(hash).Replace("-", string.Empty);
                List<char> charlist = new List<char>(uppercaseString.ToCharArray());
                char[] lowercaseCharlist = charlist.Select(c => char.ToLower(c)).ToArray();

                return new string(lowercaseCharlist);
            }
        }
    }
}
