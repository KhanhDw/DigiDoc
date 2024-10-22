using System;
using System.Security.Cryptography;
using System.Text;

namespace WebApplication1.Helper
{
    public class MD5Convert
    {
        public static string GetMd5Hash(string repasswordregister)
        {
            // Chuyển đổi chuỗi thành mảng byte
            byte[] inputBytes = Encoding.ASCII.GetBytes(repasswordregister);

            // Tạo đối tượng MD5
            using (MD5 md5 = MD5.Create())
            {
                // Tính toán giá trị băm
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Chuyển đổi mảng byte thành chuỗi hex
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
        }
    }
}