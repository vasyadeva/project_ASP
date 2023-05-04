using DataLayer;
using DataLayer.Entities;
using LinkedNewsChatApp.Hubs;

namespace ServiceLayer
{
    public class ChatOperations
    {
        private readonly LinkedNewsRepository _repository;
        private readonly LoginOperations _loginOperator;

        public ChatOperations(LinkedNewsRepository repository, LoginOperations loginOperator)
        {
            _repository = repository;
            _loginOperator = loginOperator;
        }

        public List<string>? GetListOfGroups(string name)
        {
            return _repository.GetListOfGroups(name);
        }
        public List<string>? GetAllListOfGroups()
        {
            return _repository.GetAllListOfGroups();
        }

        public void AddGroup(string groupName, string userid, string username)
        {
            //var group = new Group() { GroupName = groupName };
            var group = new HubGroup() { Name = groupName };
            var user = new HubUser() {  UserIdentifier = userid, Name=username, GroupChatName=groupName};
            _repository.AddGroup(group,user);
        }

        public void addMember(string groupName, string userid, string username)
        {
            var user = new HubUser() { UserIdentifier = userid, Name = username, GroupChatName = groupName };
            _repository.addMember(user);
        }
        /*public Group? GetGroupByName(string groupName)
        {
            var groupList = _repository.GetListOfGroups();
            var foundGroup = groupList.FirstOrDefault(g => g.GroupName == groupName);
            return foundGroup;
        }*/
        public void AddGroupMessage(string UserName, string time, string message,string groupname, int avaid)
        {
            var addmessage = new HubGroupMessage { FromUserName = UserName, Time = time, Message = message , GroupChatName = groupname,AvaId= avaid};
            _repository.AddGroupMessage(addmessage);
        }
        public void AddPrivateMessage(string UserName, string time, string message, string chatname, int avaid)
        {
            var addmessage = new HubMessage { FromUserName = UserName, Time = time, Message = message,  PrivateChatName = chatname, AvaId = avaid };
            _repository.AddPrivateMessage(addmessage);
        }
        public List<HubGroupMessageMdl> GetGroupMessages(string groupName) {
            return _repository.GetGroupMessages(groupName);
  
        }
        public List<HubMessageMdl> GetPrivateMessages(string chatName)
        {
            return _repository.GetPrivateMessages(chatName);

        }
        public List<string> GetFriends(string username)
        {
            return _repository.GetFriends(username);
        }
        public List<HubFriend> GetHubFriends(string username)
        {
            return _repository.GetHubFriends(username);
        }
        public void CheckMainChat()
        {
            _repository.CheckMainChat();
        }
        public int AvaId(string Username)
        {
           return _repository.GetAvatarId(Username);
        }
        public string GetReg(string Username)
        {
            return _repository.Getregion(Username);
        }
        public void AddChat(string groupName)
        {
            var grpname = new PrivateChat { Name = groupName };
            _repository.AddPrivateGroup(grpname);
        }
        public void AddFriend(string username, string friendname)
        {
            var friend = new UserFriends { username = username, friend = friendname };
            _repository.AddFriend(friend);
        }
        public void DeleteFriend(string username, string friendname)
        {
            var friend = new UserFriends { username = username, friend = friendname };
            _repository.DeleteFriend(friend);
        }
        public void LeaveFromGroup(string username, string group)
        {
            _repository.LeaveFromGroup(username, group);
        }
        public List<string> GetUsers(string username)
        {
            return _repository.GetUsers(username);  
        }
        public bool CheckBan(string username)
        { return _repository.CheckBan(username);}
        public int GetTermBan(string username)
        {
            return _repository.GetTermBan(username);
        }
      
    }
}