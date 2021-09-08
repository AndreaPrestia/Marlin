using JWT.Algorithms;
using JWT.Builder;
using Marlin.Core.Entities;
using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Marlin.Core.Common
{
    public static class Helper
    {
        private static readonly JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true, AllowTrailingCommas = true };

        public static T Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, options);
        }

        public static object Deserialize(string json, Type returnType)
        {
            return JsonSerializer.Deserialize(json, returnType);
        }

        public static string Serialize(object o)
        {
            return JsonSerializer.Serialize(o, options);
        }

        public static string GetBearer(string secret, User user)
        {
            string token = JwtBuilder.Create()
               .WithAlgorithm(new HMACSHA256Algorithm())
               .WithSecret(secret)
               .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(12).ToUnixTimeSeconds())
               .AddClaim("user", user)
               .Encode();

            return token;
        }

        public static long GetUnixTimestampMs(DateTime dateTime) => (long)(TimeZoneInfo.ConvertTimeToUtc(dateTime) - new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalMilliseconds;
        public static long GetUnixTimestamp(DateTime dateTime) => (long)(TimeZoneInfo.ConvertTimeToUtc(dateTime) - new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalSeconds;
        public static DateTime GetFromUnixTimestampMs(long timestamp) => TimeZoneInfo.ConvertTimeFromUtc(new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddMilliseconds(timestamp), TimeZoneInfo.Local);
        public static DateTime GetFromUnixTimestamp(long timestamp) => TimeZoneInfo.ConvertTimeFromUtc(new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddSeconds(timestamp), TimeZoneInfo.Local);

        internal static string GetSha256(string text)
        {
            byte[] b = Encoding.Default.GetBytes(text);

            using (SHA256 calculator = SHA256.Create())
            {
                byte[] c = calculator.ComputeHash(b);

                StringBuilder stringBuilder = new StringBuilder();

                for (int i = 0; i < c.Length; i++)
                {
                    stringBuilder.Append($"{c[i]:X2}");
                }

                return stringBuilder.ToString();
            }
        }

        internal static string GetRandomString()
        {
            int length = Settings.Get("PasswordLength", 10);

            string valid = Settings.Get("PasswordValidChars", @"ABCDEFGHIJKLMNOPQRSTUVWYZabcdefghijklmnopqrstuvwxyz0123456789%$/();\|");

            StringBuilder sb = new StringBuilder();

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] uintBuffer = new byte[sizeof(uint)];

                while (length-- > 0)
                {
                    rng.GetBytes(uintBuffer);
                    uint num = BitConverter.ToUInt32(uintBuffer, 0);
                    sb.Append(valid[(int)(num % (uint)valid.Length)]);
                }
            }

            return sb.ToString();
        }

        internal static void SendSms(string message, string mobile)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (string.IsNullOrEmpty(mobile))
            {
                throw new ArgumentNullException(nameof(mobile));
            }

            if (mobile.StartsWith("+"))
            {
                mobile = mobile.Substring(1);
            }

            string skebbyHost = Settings.Get<string>("Skebby.Host");

            string skebbyUsername = Settings.Get<string>("Skebby.Username");

            string skebbyPassword = Settings.Get<string>("Skebby.Password");

            string[] auth = null;

            using (WebClient wb = new WebClient())
            {
                var response = wb.DownloadString(skebbyHost + "login?username=" + skebbyUsername + "&password=" + skebbyPassword);
                auth = response.Split(';');
            }

            using (WebClient wb = new WebClient())
            {
                // Setting the encoding is required when sending UTF8 characters!
                wb.Encoding = System.Text.Encoding.UTF8;

                wb.Headers.Set(HttpRequestHeader.ContentType, "application/json");
                wb.Headers.Add("user_key", auth[0]);
                wb.Headers.Add("session_key", auth[1]);

                string json = System.Text.Json.JsonSerializer.Serialize(new
                {
                    Message = message,
                    MessageType = "GP",
                    Recipient = new string[] { $"+{mobile}"}
                });

                string responseBody = wb.UploadString(skebbyHost + "sms", "POST", json);

                dynamic smsResponse = System.Text.Json.JsonSerializer.Deserialize<dynamic>(responseBody);

                if (!"OK".Equals(smsResponse.Result))
                {
                    throw new InvalidOperationException($"Error sending sms for user {mobile} \n\r {smsResponse.Result}");
                }
            }
        }

        internal static T CreateInstance<T>(string typeName)
        {
            Type type = Type.GetType(typeName);

            if (type == null)
            {
                throw new TypeLoadException(typeName);
            }

            return (T)Activator.CreateInstance(type);
        }
    }
}
