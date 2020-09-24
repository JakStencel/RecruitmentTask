using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RecruitmentTask.Helpers;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace RecruitmentTask.Services
{
    public interface ISSOServcie
    {
        string GetCommentoSSOPath(string payload, string hmac);
        bool AreHmacWithEncodedTokenEqual(string token, string hmac);
        byte[] GetBytes(string collectionOfChars);
        string GetHashedPayloadString(JObject payloadJson);
    }

    public class SSOServcie : ISSOServcie
    {
        private readonly IWebConfigReader _webConfigReader;

        public SSOServcie(IWebConfigReader webConfigReader)
        {
            _webConfigReader = webConfigReader;
        }
        public string GetCommentoSSOPath(string payload, string hmac)
        {
            var commentoSSOPath = _webConfigReader.GetWebConfigSetting("CommentoSSOPath");
            return $"{commentoSSOPath}?payload={payload}&hmac={hmac}";
        }

        public bool AreHmacWithEncodedTokenEqual(string token, string hmac)
        {
            byte[] keyByte = GetBytes(_webConfigReader.GetWebConfigSetting("SecretKey"));
            byte[] tokenByte = GetBytes(token);
            byte[] hmacByte = GetBytes(hmac);

            var tokenComputedHash = GetComputedHashBasedOnSHA256(keyByte, tokenByte);

            return tokenComputedHash.SequenceEqual(hmacByte);
        }

        public string GetHashedPayloadString(JObject payloadJson)
        {
            string serializedJson = JsonConvert.SerializeObject(payloadJson);
            string encodedJson = Convert.ToBase64String(GetBytes(serializedJson));

            byte[] keyByte = GetBytes(_webConfigReader.GetWebConfigSetting("SecretKey"));
            byte[] hashedJson = GetComputedHashBasedOnSHA256(keyByte, GetBytes(encodedJson));

            return Get64HexStringFrom32ByteArray(hashedJson);
        }

        public byte[] GetBytes(string collectionOfChars) => new ASCIIEncoding().GetBytes(collectionOfChars);

        public string Get64HexStringFrom32ByteArray(byte[] hashedJson)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (byte b in hashedJson)
                stringBuilder.AppendFormat("{0:x2}", b);

            return stringBuilder.ToString();
        }

        private byte[] GetComputedHashBasedOnSHA256(byte[] key, byte[] token)
        {
            using (var hmacsha256 = new HMACSHA256(key))
            {
                return hmacsha256.ComputeHash(token);
            }
        }
    }
}