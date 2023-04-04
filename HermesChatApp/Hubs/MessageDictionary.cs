using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace LinkedNewsChatApp.Hubs
{
    public class MessageDictionary
    {
        private Dictionary<string, List<HubMessage>> dictionary;
        private readonly string filePath = "dictionary.json";
        private readonly string ekey = "b14ca5898a4e4133bbce2ea2315a1916";

        public MessageDictionary()
        {
            dictionary = LoadDictionaryFromFile() ?? new Dictionary<string, List<HubMessage>>();
        }

        public void Add(string key, HubMessage value)
        {
            dictionary ??= new Dictionary<string, List<HubMessage>>();

            if (dictionary.TryGetValue(key, out List<HubMessage> messageList))
            {
                /*if (messageList.Count >= 10)
                {
                    messageList.RemoveAt(0);
                }*/

                messageList.Add(value);
            }
            else
            {
                messageList = new List<HubMessage> { value };
                dictionary.Add(key, messageList);
            }

            SaveDictionaryToFile();
        }


        public List<HubMessage> GetLastMessageList(string key)
        {
            if (dictionary != null && dictionary.TryGetValue(key, out List<HubMessage> messageList))
            {
                return messageList;
            }
            else
            {
                return new List<HubMessage>();
            }
        }

        private Dictionary<string, List<HubMessage>> LoadDictionaryFromFile()
        {
            if (File.Exists(filePath))
            {
                string encryptedJson = File.ReadAllText(filePath);
                string json = DecryptString(ekey, encryptedJson);
                return JsonConvert.DeserializeObject<Dictionary<string, List<HubMessage>>>(json);
            }

            return new Dictionary<string, List<HubMessage>>();
        }

        private void SaveDictionaryToFile()
        {
            string json = JsonConvert.SerializeObject(dictionary);
            string encryptedJson = EncryptString(ekey, json);
            File.WriteAllText(filePath, encryptedJson);
        }

        public static string EncryptString(string ekey, string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(ekey);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        public static string DecryptString(string ekey, string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(ekey);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }

}
