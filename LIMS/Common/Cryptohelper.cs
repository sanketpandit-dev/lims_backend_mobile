using DataObject;                     
using Newtonsoft.Json;                
using System.Security.Cryptography;   
using System.IO;                     

using Org.BouncyCastle.Crypto;               
using Org.BouncyCastle.Crypto.Parameters;    
using Org.BouncyCastle.OpenSsl;               
using Org.BouncyCastle.Security;             



namespace LIMS.Common
{
    public class Cryptohelper
    {
        public static RSAParameters PublicKey;
        public static RSAParameters PrivateKey;

        private static string _publicKeyPath;
        private static string _privateKeyPath;
        static Cryptohelper()
        {
            //string publicKeyPath = "Keys/public_key.pem";
            //string privateKeyPath = "Keys/private_key.pem";

            //PublicKey = LoadRsaPublicKey(publicKeyPath);
            //PrivateKey = LoadRsaPrivateKey(privateKeyPath);
        }
        public static void Initialize(string publicKeyPath, string privateKeyPath)
        {
            _publicKeyPath = publicKeyPath;
            _privateKeyPath = privateKeyPath;
            PublicKey = LoadRsaPublicKey(_publicKeyPath);
            PrivateKey = LoadRsaPrivateKey(_privateKeyPath);
        }
        private static RSAParameters LoadRsaPublicKey(string filePath)
        {
            using var reader = File.OpenText(filePath);
            var pemReader = new PemReader(reader);
            var keyParams = (RsaKeyParameters)pemReader.ReadObject();
            return DotNetUtilities.ToRSAParameters(keyParams);
        }

        private static RSAParameters LoadRsaPrivateKey(string filePath)
        {
            using var reader = File.OpenText(filePath);
            var pemReader = new PemReader(reader);
            var keyPair = (AsymmetricCipherKeyPair)pemReader.ReadObject();
            var privateKeyParams = (RsaPrivateCrtKeyParameters)keyPair.Private;
            return DotNetUtilities.ToRSAParameters(privateKeyParams);
        }



        // ========== RSA ==========
        public static byte[] RSA_Encrypt(byte[] data, RSAParameters publicKey)
        {
            using RSA rsa = RSA.Create();
            rsa.ImportParameters(publicKey);
            return rsa.Encrypt(data, RSAEncryptionPadding.OaepSHA1);
        }

        public static byte[] RSA_Decrypt(byte[] data, RSAParameters privateKey)
        {
            using RSA rsa = RSA.Create();
            rsa.ImportParameters(privateKey);
            return rsa.Decrypt(data, RSAEncryptionPadding.OaepSHA1);
        }

        // Rohit R
        // ========== AES ==========
        //public static (byte[] EncryptedData, byte[] IV, byte[] Key) AES_Encrypt(string plainText)
        //{
        //    using Aes aes = Aes.Create();
        //    aes.GenerateIV();
        //    aes.GenerateKey();

        //    ICryptoTransform encryptor = aes.CreateEncryptor();
        //    using MemoryStream ms = new();
        //    using CryptoStream cs = new(ms, encryptor, CryptoStreamMode.Write);
        //    using StreamWriter sw = new(cs);
        //    sw.Write(plainText);
        //    sw.Close();

        //    return (ms.ToArray(), aes.IV, aes.Key);
        //}


        public static (byte[] EncryptedData, byte[] IV, byte[] Key) AES_Encrypt(string plainText , byte[] AesKey, byte[] Iv)
        {
            using Aes aes = Aes.Create();
            aes.Key = AesKey;
            aes.IV = Iv;

            ICryptoTransform encryptor = aes.CreateEncryptor();
            using MemoryStream ms = new();
            using CryptoStream cs = new(ms, encryptor, CryptoStreamMode.Write);
            using StreamWriter sw = new(cs);
            sw.Write(plainText);
            sw.Close();

            return (ms.ToArray(), aes.IV, aes.Key);
        }



        public static string AES_Decrypt(byte[] cipherText, byte[] key, byte[] iv)
        {
            using Aes aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;

            ICryptoTransform decryptor = aes.CreateDecryptor();
            using MemoryStream ms = new(cipherText);
            using CryptoStream cs = new(ms, decryptor, CryptoStreamMode.Read);
            using StreamReader sr = new(cs);
            return sr.ReadToEnd();
        }

        // ========== Utility for API ==========

        //public static LoginDO DecryptLoginRequest(EncryptedRequest request)
        //{
        //    byte[] aesKey = RSA_Decrypt(Convert.FromBase64String(request.EncryptedAESKey), PrivateKey);
        //    byte[] iv = Convert.FromBase64String(request.IV);
        //    byte[] encryptedData = Convert.FromBase64String(request.EncryptedData);

        //    string json = AES_Decrypt(encryptedData, aesKey, iv);
        //    return JsonConvert.DeserializeObject<LoginDO>(json);
        //}

        //public static EncryptedResponse EncryptLoginResponse(loginResponseDO response)
        //{
        //    string responseJson = JsonConvert.SerializeObject(response);
        //    var (encryptedData, iv, key) = AES_Encrypt(responseJson);
        //    byte[] encryptedAESKey = RSA_Encrypt(key, PublicKey);

        //    return new EncryptedResponse
        //    {
        //        EncryptedData = Convert.ToBase64String(encryptedData),
        //        IV = Convert.ToBase64String(iv),
        //        EncryptedAESKey = Convert.ToBase64String(encryptedAESKey)
        //    };
        //}

        //public static EncryptedResponse EncryptResponse<T>(T responseObject)
        //{
        //    string responseJson = JsonConvert.SerializeObject(responseObject);
        //    var (encryptedData, iv, key) = AES_Encrypt(responseJson);
        //    byte[] encryptedAESKey = RSA_Encrypt(key, PublicKey);

        //    return new EncryptedResponse
        //    {
        //        EncryptedData = Convert.ToBase64String(encryptedData),
        //        IV = Convert.ToBase64String(iv),
        //        EncryptedAESKey = Convert.ToBase64String(encryptedAESKey)
        //    };
        //}


        // Rohit R aes encrypt
        public static EncryptedResponse EncryptResponse<T>(T responseObject, byte[] AesKey , byte[] Iv)
        {
            string responseJson = JsonConvert.SerializeObject(responseObject);
            var (encryptedData, iv, key) = AES_Encrypt(responseJson,AesKey,Iv);
            byte[] encryptedAESKey = RSA_Encrypt(key, PublicKey);

            return new EncryptedResponse
            {
                EncryptedData = Convert.ToBase64String(encryptedData),
                IV = Convert.ToBase64String(iv),
                EncryptedAESKey = Convert.ToBase64String(encryptedAESKey)
            };
        }


        // 17-08-2025 Rohit R
        public static DecryptedJsonDataWithKey DecryptRequest<T>(EncryptedRequest request)
        {
            byte[] aesKey = RSA_Decrypt(Convert.FromBase64String(request.EncryptedAESKey), PrivateKey);
            byte[] iv = Convert.FromBase64String(request.IV);
            byte[] encryptedData = Convert.FromBase64String(request.EncryptedData);

            string json = AES_Decrypt(encryptedData, aesKey, iv);

            DecryptedJsonDataWithKey data = new DecryptedJsonDataWithKey
            {
                DecryptedJsonData = json,
                ReqAesKey = aesKey,
                Iv = iv,
            };
            return data;

        }


        public static string GetPublicKeyBase64()
        {
            using var rsa = RSA.Create();
            rsa.ImportParameters(PublicKey);
            var keyBytes = rsa.ExportSubjectPublicKeyInfo(); 
            return Convert.ToBase64String(keyBytes);
        }
    }
}
