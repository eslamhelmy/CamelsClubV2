using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CamelsClub.Data.Helpers
{
    public class SecurityHelper
    {

        private string allChars = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm1234567890@#$";
        private const string _alg = "HmacSHA256";
        private const string _salt = "VCDGIl7AAZ2Bhu0MDFJ0"; // Generated at https://www.random.org/strings
        #region Tokens
        public static string GenerateToken(int userID)
        {
            //Now we will create token from Account ID , User Agent , Created Date 
            string hash = string.Join(":", new string[] { userID.ToString(), Guid.NewGuid().ToString(), DateTime.Now.Ticks.ToString() });
            string hashLeft = "";
            string hashRight = "";
            using (HMAC hmac = HMACSHA256.Create(_alg))
            {
                hmac.Key = Encoding.UTF8.GetBytes(_salt);
                hmac.ComputeHash(Encoding.UTF8.GetBytes(hash));
                hashLeft = Convert.ToBase64String(hmac.Hash);
                //This is public part at access token to validate experation date.
                hashRight = string.Join(":", new string[] { userID.ToString(), DateTime.Now.AddDays(10).Ticks.ToString() });
            }
            string accessToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Join(":", hashLeft, hashRight)));

            //Save accessToken to DB related to account

            return accessToken;
        }
        public static string GetHashedString(string word)
        {
            string key = string.Join(":", new string[] { word, _salt });
            using (HMAC hmac = HMACSHA256.Create(_alg))
            {
                // Hash the key.
                hmac.Key = Encoding.UTF8.GetBytes(_salt);
                hmac.ComputeHash(Encoding.UTF8.GetBytes(key));
                return Convert.ToBase64String(hmac.Hash);
            }
        }
        public static bool IsTokenExpired(string token)
        {
            return false;
            bool validToken = false;
            try
            {
                // Base64 decode the string, obtaining the token:account id:experation.
                string key = Encoding.UTF8.GetString(Convert.FromBase64String(token));
                // Split the parts.
                string[] parts = key.Split(new char[] { ':' });
                if (parts.Length == 3)
                {
                    // Get the hash message, username, and timestamp.
                    string hash = parts[0];
                    int userID = int.Parse(parts[1]);
                    long ticks = long.Parse(parts[2]);
                    DateTime expirationTime = new DateTime(ticks);
                    // Ensure the timestamp is valid.
                    //First level of Validation
                    return DateTime.Now < expirationTime;

                }
            }
            catch
            {
                validToken = false;
            }
            return validToken;
        }
        public static int GetUserIDFromToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                return 0;
            int userID = 0;
            try
            {
                // Base64 decode the string, obtaining the token:account id:experation.
                string key = Encoding.UTF8.GetString(Convert.FromBase64String(token));
                // Split the parts.
                string[] parts = key.Split(new char[] { ':' });
                if (parts.Length == 3)
                {
                    // Get the hash message, username, and timestamp.
                    string hash = parts[0];
                    userID = int.Parse(parts[1]);
                    long ticks = long.Parse(parts[2]);
                }
            }
            catch (Exception ex)
            {

            }
            return userID;
        }

        #endregion

        #region Encryption & Decryption


        public static string Encrypt(string text)
        {
            string result;
            if (text == "")
            {
                result = "";
            }
            else
            {
                UTF8Encoding uTF8Encoding = new UTF8Encoding();
                MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
                byte[] key = mD5CryptoServiceProvider.ComputeHash(uTF8Encoding.GetBytes(_salt));
                TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider();
                tripleDESCryptoServiceProvider.Key = key;
                tripleDESCryptoServiceProvider.Mode = CipherMode.ECB;
                tripleDESCryptoServiceProvider.Padding = PaddingMode.PKCS7;
                byte[] bytes = uTF8Encoding.GetBytes(text);
                byte[] inArray;
                try
                {
                    ICryptoTransform cryptoTransform = tripleDESCryptoServiceProvider.CreateEncryptor();
                    inArray = cryptoTransform.TransformFinalBlock(bytes, 0, bytes.Length);
                }
                finally
                {
                    tripleDESCryptoServiceProvider.Clear();
                    mD5CryptoServiceProvider.Clear();
                }
                result = Convert.ToBase64String(inArray);
            }
            return result;
        }

        public static string Decrypt(string text)
        {
            string result;
            if (text == "")
            {
                result = "";
            }
            else
            {
                UTF8Encoding uTF8Encoding = new UTF8Encoding();
                MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
                byte[] key = mD5CryptoServiceProvider.ComputeHash(uTF8Encoding.GetBytes(_salt));
                TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider();
                tripleDESCryptoServiceProvider.Key = key;
                tripleDESCryptoServiceProvider.Mode = CipherMode.ECB;
                tripleDESCryptoServiceProvider.Padding = PaddingMode.PKCS7;
                byte[] array = Convert.FromBase64String(text);
                byte[] bytes;
                try
                {
                    ICryptoTransform cryptoTransform = tripleDESCryptoServiceProvider.CreateDecryptor();
                    bytes = cryptoTransform.TransformFinalBlock(array, 0, array.Length);
                }
                catch (Exception ex)
                {
                    bytes = null;
                }
                finally
                {
                    tripleDESCryptoServiceProvider.Clear();
                    mD5CryptoServiceProvider.Clear();
                }
                result = uTF8Encoding.GetString(bytes);
            }
            return result;
        }

        public static bool IsStrongPassword(string password)
        {
            string pattern = "^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{8,15}$";
            bool result;
            try
            {
                Regex.Match("", pattern);
            }
            catch (ArgumentException)
            {
                result = false;
                return result;
            }
            result = true;
            return result;
        }
        #endregion

        public static int GetRandomNumber()
        {
            return new Random().Next(1000, 9999);
        }
    }
}
