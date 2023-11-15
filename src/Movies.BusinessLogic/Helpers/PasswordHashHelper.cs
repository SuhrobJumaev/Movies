
using System.Security.Cryptography;
using System.Text;

namespace Movies.BusinessLogic.Helpers;

public static class PasswordHashHelper
{
    private static string _secretKey = "!@а#ф$S$п%д";

    public static string PasswordHash(string password, string email)
    {
        using (SHA256 sha256 = SHA256.Create()) 
        {           
            byte[] inputBytes = Encoding.UTF8.GetBytes(password + email + _secretKey);
            byte[] hashBytes = sha256.ComputeHash(inputBytes);

            StringBuilder builder = new StringBuilder();
            foreach (byte b in hashBytes)
            {
                builder.Append(b.ToString("x2"));
            }

            return builder.ToString();
        }
    }
}
