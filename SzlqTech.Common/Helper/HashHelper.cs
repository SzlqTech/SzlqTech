
using System.Security.Cryptography;
using System.Text;

namespace SzlqTech.Common.Helper
{
    /// <summary>
    /// 消息摘要/校验算法等
    /// </summary>
    public class HashHelper
    {
        /// <summary>
        /// 创建md5
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string CreateMD5(string source)
        {
            // Use input string to calculate MD5 hash
            using MD5 md5 = MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(source);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // return Convert.ToHexString(hashBytes); // .NET 5 +
            // Convert the byte array to hexadecimal string prior to .NET 5
            StringBuilder sb = new System.Text.StringBuilder();
            foreach (var b in hashBytes)
            {
                sb.Append(b.ToString("X2"));
            }
            return sb.ToString();
        }

        public static string CreateSHA1(string source)
        {
            using SHA1 md5 = SHA1.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(source);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // return Convert.ToHexString(hashBytes); // .NET 5 +
            // Convert the byte array to hexadecimal string prior to .NET 5
            StringBuilder sb = new System.Text.StringBuilder();
            foreach (var b in hashBytes)
            {
                sb.Append(b.ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
