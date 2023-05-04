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
        public List<string>? GetListOfGroups(string name);
        public List<string>? GetAllListOfGroups();
        public void addMember(HubUser user);
        public void AddGroup(HubGroup group,HubUser user);
        public void AddGroupMessage(HubGroupMessage message);
        public void AddPrivateMessage(HubMessage message);
        public void AddFriend(UserFriends friends);
        public List<HubMessageMdl> GetPrivateMessages(string chatName);
        public List<HubGroupMessageMdl> GetGroupMessages(string groupName);
        public List<string> GetFriends(string username);
        public List<HubFriend> GetHubFriends(string username);
        public List<string> GetUsers(string username);
        public void CheckMainChat();
        public int GetAvatarId(string Username);
        public string Getregion(string Username);
        public void AddPrivateGroup(PrivateChat group);
        public void LeaveFromGroup(string username,string group);
        public void DeleteFriend(UserFriends friends);
        public bool CheckBan(string username);
        public void Ban(string username,int term);
        public void UnBan(string username);
        public int GetTermBan(string username);
        public void DeleteMessage(string username,string message);
        public void DeleteMessages(string username, string message);

    }
}
