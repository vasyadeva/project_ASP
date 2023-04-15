using DataLayer.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinkedNewsChatApp.Hubs
{
    public class HubMessage
    {
        public int Id { get; set; }
        public string FromUserName { get; set; }
        public string Time { get; set; }
        public string Message { get; set; }
        public int AvaId { get; set; }

        [ForeignKey(nameof(privateChat))]
        public string PrivateChatName { get; set; }

        public PrivateChat privateChat { get; set; }
    }
}
