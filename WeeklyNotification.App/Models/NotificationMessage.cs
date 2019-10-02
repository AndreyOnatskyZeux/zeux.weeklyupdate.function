using Newtonsoft.Json;

namespace WeeklyNotification.App.Models
{
    public class NotificationMessage
    {
        [JsonProperty("deviceToken")]
        public string DeviceToken { get; set; }

        [JsonProperty("deviceType")]
        public string DeviceType { get; set; }

        [JsonProperty("customerId")]
        public int CustomerId { get; set; }

        [JsonProperty("message")]
        public MessageContent Message { get; set; }
    }

    public class MessageContent

    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("badge")]
        public int Badge { get; set; }

        [JsonProperty("sound")]
        public string Sound { get; set; }

        [JsonProperty("data")]
        public object Data { get; set; }
    }
}