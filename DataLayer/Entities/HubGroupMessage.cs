using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
    public class HubGroupMessage
    {
        public int Id { get; set; }
        public string FromUserName { get; set; }
        public string Time { get; set; }
        public string Message { get; set; }
        public int AvaId { get; set; }


        [ForeignKey(nameof(hubGroup))]
        public string GroupChatName { get; set; }

        public HubGroup hubGroup { get; set; }
    }
}
