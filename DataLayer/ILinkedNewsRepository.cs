using DataLayer.Entities;
using LinkedNewsChatApp.Hubs;

namespace DataLayer
{
    public interface ILinkedNewsRepository
    {
        public List<User> GetListOfUsers();
        public User UpdateUserPassword(User user);
        public void Register(User user);
        //public List<Group> GetListOfGroups();
        //public void AddGroup(Group group);
        public List<string>? GetListOfGroups();
        public void addMember(HubUser user);
        public void AddGroup(HubGroup group,HubUser user);
        public void AddGroupMessage(HubGroupMessage message);
        public void AddPrivateMessage(HubMessage message);
        public List<HubMessageMdl> GetPrivateMessages(string chatName);
        public List<HubGroupMessageMdl> GetGroupMessages(string groupName);
        public void CheckMainChat();
        public int GetAvatarId(string Username);

    }
}
