using System.ComponentModel.DataAnnotations;

namespace LinkedNewsChatApp.Models
{
    public class MessageModel
    {
        [Key]
        public int MessageId { get; set; }
        public string? MessageFrom { get; set; }
        public string? MessageToUser { get; set; }
        public string? MessageToGroup { get; set; }
        public string? MessageContent { get; set; }
        public DateTime MessageTime { get; set; }
    }
}
