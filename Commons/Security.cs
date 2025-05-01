using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DocumentManager.Common.Schemas;
using Microsoft.IdentityModel.Tokens;
using vegetarian.Common;

namespace vegetarian.Commons
{
    public class Security
    {
        /// <summary>
        /// Mã hóa SHA256 của 1 chuỗi có thêm chuối khóa đầu và cuối.
        /// <para>Author: QuyPN</para>
        /// <para>Created: 15/02/2020</para>
        /// </summary>
        /// <param name="str">Chuỗi cần mã hóa.</param>
        /// <param name="firstStr">Chuỗi bảo mật đầu</param>
        /// <param name="lastStr">Chuỗi bảo mật cuối</param>
        /// <returns>Chuỗi sau khi đã được mã hóa.</returns>
        public static string GetSHA256(string str, string firstStr = "", string lastStr = "")
        {
            str = firstStr + str + lastStr;
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(str));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        /// <summary>
        /// Mã hóa MD5 của 1 chuỗi có thêm chuối khóa đầu và cuối.
        /// <para>Author: QuyPN</para>
        /// <para>Created: 15/02/2020</para>
        /// </summary>
        /// <param name="str">Chuỗi cần mã hóa.</param>
        /// <param name="firstStr">Chuỗi bảo mật đầu</param>
        /// <param name="lastStr">Chuỗi bảo mật cuối</param>
        /// <returns>Chuỗi sau khi đã được mã hóa.</returns>
        public static string GetMD5(string str, string firstStr = "", string lastStr = "")
        {
            str = firstStr + str + lastStr;
            string str_md5 = "";
            byte[] mang = System.Text.Encoding.UTF8.GetBytes(str);
            MD5 my_md5 = MD5.Create();
            mang = my_md5.ComputeHash(mang);
            foreach (byte b in mang)
            {
                str_md5 += b.ToString("x2");
            }
            return str_md5;
        }

        /// <summary>
        /// Mã hóa MD5 của 1 chuỗi không có thêm chuối khóa đầu và cuối.
        /// <para>Author: QuyPN</para>
        /// <para>Created: 15/02/2020</para>
        /// </summary>
        /// <param name="str">Chuỗi cần mã hóa</param>
        /// <returns>Chuỗi sau khi đã được mã hóa</returns>
        public static string GetSimpleMD5(string str)
        {
            string str_md5 = "";
            byte[] mang = System.Text.Encoding.UTF8.GetBytes(str);
            MD5 my_md5 = MD5.Create();
            mang = my_md5.ComputeHash(mang);
            foreach (byte b in mang)
            {
                str_md5 += b.ToString("x2");
            }
            return str_md5;
        }

        /// <summary>
        /// Mã hóa base64 của 1 chuỗi
        /// <para>Author: QuyPN</para>
        /// <para>Created: 15/02/2020</para>
        /// </summary>
        /// <param name="plainText">Chuỗi cần mã hóa</param>
        /// <returns>Chuỗi sau khi mã hóa</returns>
        public static string Base64Encode(string plainText)
        {
            try
            {
                if (String.IsNullOrEmpty(plainText))
                {
                    return "";
                }
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
                string base64Str = Convert.ToBase64String(plainTextBytes);
                int endPos = 0;
                for (endPos = base64Str.Length; endPos > 0; endPos--)
                {
                    if (base64Str[endPos - 1] != '=')
                    {
                        break;
                    }
                }
                int numberPaddingChars = base64Str.Length - endPos;
                base64Str = base64Str.Replace("+", "-");
                base64Str = base64Str.Replace("/", "_");
                base64Str = base64Str.Substring(0, endPos);
                base64Str = $"{base64Str}{numberPaddingChars}";
                return base64Str;
            }
            catch
            {
                return plainText;
            }
        }

        /// <summary>
        /// Chuyển mã base64 về chuỗi trước khi mã hóa.
        /// <para>Author: QuyPN</para>
        /// <para>Created: 15/02/2020</para>
        /// </summary>
        /// <param name="base64EncodedData">Chuỗi mã hóa</param>
        /// <returns>Chuỗi sau khi giải mã</returns>
        public static string Base64Decode(string base64EncodedData)
        {
            try
            {
                if (String.IsNullOrEmpty(base64EncodedData))
                {
                    return "";
                }
                base64EncodedData = base64EncodedData.Replace("-", "+");
                base64EncodedData = base64EncodedData.Replace("_", "/");
                int numberPaddingChars = 0;
                try
                {
                    numberPaddingChars = Convert.ToInt32(base64EncodedData.Substring(base64EncodedData.Length - 1, 1));
                }
                catch { }
                base64EncodedData = base64EncodedData.Substring(0, base64EncodedData.Length - 1);
                for (int i = 0; i < numberPaddingChars; i++)
                {
                    base64EncodedData += "=";
                }
                var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
                return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            }
            catch
            {
                return base64EncodedData;
            }
        }

        /// <summary>
        /// Render token dùng để xác thực đăng nhập và lưu thông tin user đăng nhập
        /// <para>Created at: 18/04/2021</para>
        /// <para>Created by: QuyPN</para>
        /// </summary>
        /// <param name="data">Dữ liệu cần thiết để tạo token</param>
        /// <returns>Chuỗi token được tạo cho user</returns>
        public static string GenerateJWTCode(JwtData data)
        {
            var now = DateTime.Now;
            var configuration = StartupState.Instance.Configuration.GetSection("JWTSetting");
            // Khởi tạo Claim
            var claims = new Claim[] {
                new Claim ("UserId", data.UserId.ToString()),
                new Claim ("Username", data.Username),
                new Claim(JwtRegisteredClaimNames.Sub, data.Username),
                new Claim (JwtRegisteredClaimNames.Jti, Guid.NewGuid ().ToString ()),
                new Claim (JwtRegisteredClaimNames.Iat, now.ToUniversalTime ().ToString (), ClaimValueTypes.Integer64)
            };

            // Khởi tạo SymmetricSecurityKey
            var symmetricKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["SecretKey"]));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                Issuer = configuration["Issuer"],
                Audience = configuration["Audience"],
                SigningCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha512Signature)
            };
            
            var tokenHandler= new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Kiểm tra chữ ký để đảm bảo data xác thực.
        /// <para>Author: QuyPN</para>
        /// <para>Created: 12/11/2022</para>
        /// </summary>
        /// <param name="data">Dữ liệu param cần kiểm tra</param>
        /// <param name="secret">Khoá kiểm tra</param>
        /// <returns>Kết quả chữ ký đúng hay không</returns>
        public static bool ValidateSignature(object data, string secret)
        {
            SortedList<String, String> paramValues = new SortedList<String, String>(new ParamsCompare());
            StringBuilder rawData = new StringBuilder();
            string secureHash = "";
            IList<PropertyInfo> properties = data.GetType().GetProperties().ToList();
            foreach (var property in properties)
            {
                var val = property.GetValue(data)?.ToString();
                if (String.IsNullOrEmpty(val) || property.Name == "SecureHash")
                {
                    if (property.Name == "SecureHash")
                    {
                        secureHash = val;
                    }
                    continue;
                }
                if (property.PropertyType == typeof(DateTimeOffset))
                {
                    paramValues.Add(property.Name, ((DateTimeOffset)property.GetValue(data)).ToString("yyyy-MM-ddTHH:mm:ss.ffffffK"));
                }
                else
                {
                    paramValues.Add(property.Name, val);
                }
            }
            foreach (KeyValuePair<string, string> kv in paramValues)
            {
                if (!String.IsNullOrEmpty(kv.Value))
                {
                    rawData.Append(kv.Value);
                }
            }
            string rspRaw = rawData.ToString();
            string myChecksum = Security.GetSHA256(rspRaw + secret);
            return myChecksum.Equals(secureHash, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Tạo chữa ký cho dữ liệu request.
        /// <para>Author: QuyPN</para>
        /// <para>Created: 12/11/2022</para>
        /// </summary>
        /// <param name="data">Dữ liệu param cần tạo chữ ký</param>
        /// <param name="secret">Khoá kiểm tra</param>
        /// <returns>Chữ ký cho dữ liệu</returns>
        public static string CreateSignature(object data, string secret)
        {
            SortedList<String, String> paramValues = new SortedList<String, String>(new ParamsCompare());
            StringBuilder rawData = new StringBuilder();
            IList<PropertyInfo> properties = data.GetType().GetProperties().ToList();
            foreach (var property in properties)
            {
                var val = property.GetValue(data)?.ToString();
                if (String.IsNullOrEmpty(val) || property.Name == "SecureHash")
                {
                    continue;
                }
                if (property.PropertyType == typeof(DateTimeOffset))
                {
                    paramValues.Add(property.Name, ((DateTimeOffset)property.GetValue(data)).ToString("yyyy-MM-ddTHH:mm:ss.ffffffK"));
                }
                else
                {
                    paramValues.Add(property.Name, val);
                }
            }
            foreach (KeyValuePair<string, string> kv in paramValues)
            {
                if (!String.IsNullOrEmpty(kv.Value))
                {
                    rawData.Append(kv.Value);
                }
            }
            string rspRaw = rawData.ToString();
            return Security.GetSHA256(rspRaw + secret);
        }

        private class ParamsCompare : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                if (x == y) return 0;
                if (x == null) return -1;
                if (y == null) return 1;
                var vnpCompare = CompareInfo.GetCompareInfo("en-US");
                return vnpCompare.Compare(x, y, CompareOptions.Ordinal);
            }
        }
    }
}