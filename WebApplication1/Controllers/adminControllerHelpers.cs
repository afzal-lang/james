using System.Security.Cryptography;
using System.Text;

namespace WebApplication1.Controllers
{
    internal static class adminControllerHelpers
    {

        // =====================================================
        //                   HASH PASSWORD
        // =====================================================
        private static string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(
                    Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                    builder.Append(bytes[i].ToString("x2"));
                return builder.ToString();
            }
        }
    }
}