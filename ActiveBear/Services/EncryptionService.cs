using System.Security.Cryptography;
using System.Text;

namespace ActiveBear.Services
{   
    public static class EncryptionService
    {
        // TODO: async?
        public static string Sha256(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                StringBuilder builder = new StringBuilder();
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}