using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Text.Json;

namespace PslibTechSaturdays.Helpers
{
    public static class TempDataExtension
    {
        public static void Set<T>(this ITempDataDictionary tempData, string key, T value)
        {
            var serialized = JsonSerializer.Serialize(value);
            tempData[key] = serialized;
        }
        public static T Get<T>(this ITempDataDictionary tempData, string key)
        {
            tempData.TryGetValue(key, out object? value);
            return value == null ? default : JsonSerializer.Deserialize<T>((string)value);
        }

        public static void AddMessage(this ITempDataDictionary tempData, string key, MessageType type, string message)
        {
            var current = tempData.Get<List<MessageData>>(key);
            if (current == default) current = new List<MessageData>();
            current.Add(new MessageData(type, message));
            tempData.Set(key, current);
        }

        public static List<MessageData> GetMessages(this ITempDataDictionary tempData, string key)
        {
            return tempData.Get<List<MessageData>>(key);
        }

        public enum MessageType
        {
            Danger = 1,
            Info,
            Success,
            Warning
        }

        public struct MessageData
        {

            public MessageType MessageType { get; set; }
            public string MessageText { get; set; }

            public MessageData(MessageType messageType, string messageText)
            {
                MessageType = messageType;
                MessageText = messageText;
            }
        }
    }
}
