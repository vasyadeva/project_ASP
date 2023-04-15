using DataLayer.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinkedNewsChatApp.Hubs
{
    public class HubUser
    {
        public int id {  get; set; }
        public string UserIdentifier { get; set; }
        public string Name { get; set; }

        [ForeignKey(nameof(hubGroup))]
        public string GroupChatName { get; set; }

        public HubGroup hubGroup { get; set; }
    }
}
