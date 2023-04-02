using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace HermesChatApp.Hubs
{
    public class MessageDictionary
    {
        private readonly Dictionary<string, List<HubMessage>> dictionary;
        private readonly string filePath = "dictionary.json";

        public MessageDictionary()
        {
            dictionary = LoadDictionaryFromFile();
        }

        public void Add(string key, HubMessage value)
        {
            if (dictionary.TryGetValue(key, out List<HubMessage> messageList))
            {
               /* if (messageList.Count >= 10)
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

        public List<HubMessage>? GetLastMessageList(string key)
        {
            dictionary.TryGetValue(key, out List<HubMessage> messageList);
            return messageList;
        }

        private Dictionary<string, List<HubMessage>> LoadDictionaryFromFile()
        {
            if (File.Exists(filePath))
            {
                string jsonString = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<Dictionary<string, List<HubMessage>>>(jsonString);
            }

            return new Dictionary<string, List<HubMessage>>();
        }

        private void SaveDictionaryToFile()
        {
            string jsonString = JsonSerializer.Serialize(dictionary);
            File.WriteAllText(filePath, jsonString);
        }
    }
}
