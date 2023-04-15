using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class HubGroupMessageMdl
    {
        public string? FromUserName { get; set; }
        public string? Time { get; set; }
        public string? Message { get; set; }
        public int AvaId { get; set; }

    }
}
