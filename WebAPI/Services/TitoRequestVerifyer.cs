using System;
using System.Security.Cryptography;
using System.Text;
using WebAPI.Configuration;
using WebAPI.Model;

namespace WebAPI.Services
{
    public class TitoRequestVerifyer : ITitoRequestVerifyer
    {
        public readonly string _webHookSecurityToken;
        public TitoRequestVerifyer(TitoConfiguration config)
        {
            _webHookSecurityToken = config.Event.WebHookSecurityToken;
        }

        public bool VerifyPayload(string payload, string signature)
        {
            return GetHash(_webHookSecurityToken, payload)
                .Equals(signature, StringComparison.InvariantCulture);
        }

        public static string GetHash(string key, string text)
        {
            Encoding encoding = new UTF8Encoding();

            byte[] textBytes = encoding.GetBytes(text);
            byte[] keyBytes = encoding.GetBytes(key);

            byte[] hashBytes;

            using (var hash = new HMACSHA256(keyBytes))
                hashBytes = hash.ComputeHash(textBytes);

            return System.Convert.ToBase64String(hashBytes);
        }
    }
}