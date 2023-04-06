using System.ComponentModel.DataAnnotations;


namespace LinkedNewsChatApp.Models
{
    public class GroupMemberModel
    {
        [Key]
        public int Id { get; set; }
        public int GroupId { get; set; }
        public  string? Username { get; set; }
    }
}
