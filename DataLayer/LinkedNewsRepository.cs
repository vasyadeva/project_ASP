using DataLayer.Context;
using DataLayer.Entities;
using LinkedNewsChatApp.Hubs;
using Microsoft.EntityFrameworkCore;

namespace DataLayer
{
    public class LinkedNewsRepository : ILinkedNewsRepository
    {
        private readonly LinkedNewsDbContext _dbContext;

        public LinkedNewsRepository(LinkedNewsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<User> GetListOfUsers()
        {
            var userList = _dbContext.Users.ToList();
            return userList;
        }

        public User UpdateUserPassword(User user)
        {
            _dbContext.Update(user);
            _dbContext.SaveChanges();

            return user;
        }

        public void Register(User user)
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
        }

        /* public List<Group> GetListOfGroups()
         {
             var groupList = _dbContext.Groups.ToList();
             return groupList;
         }*/

        /* public void AddGroup(Group group)
         {
             _dbContext.Groups.Add(group);
             _dbContext.SaveChanges();
         }*/
        public void addMember(HubUser user)
        {
            _dbContext.hubUsers.Add(user);
            _dbContext.SaveChanges();
        }
        public List<string> GetListOfGroups()
        {
            var groupList = _dbContext.hubGroups.Where(m => m.Name!= "GeneralDefaultChat").Select(g => g.Name).ToList();
            return groupList;
        }
        public void AddGroup(HubGroup group, HubUser user)
        {
            _dbContext.hubGroups.Add(group);
            _dbContext.SaveChanges();

            _dbContext.hubUsers.Add(user);
            _dbContext.SaveChanges();
        }
        public void AddGroupMessage(HubGroupMessage message) 
        {
            
            _dbContext.hubGroupMessages.Add(message);
            _dbContext.SaveChanges();
        }

        public void AddPrivateMessage(HubMessage message)
        {

            _dbContext.HubMessages.Add(message);
            _dbContext.SaveChanges();
        }

        public List<HubMessageMdl> GetPrivateMessages(string chatName)
        {
            var messages = _dbContext.HubMessages
                .Where(m => m.PrivateChatName == chatName)
                .Select(m => new HubMessageMdl
                {
                    FromUserName = m.FromUserName,
                    Message = m.Message,
                    Time = m.Time,
                    AvaId = m.AvaId
                })
                .ToList();

            return messages;
        }

        public List<HubGroupMessageMdl> GetGroupMessages(string groupName)
        {
            var messages = _dbContext.hubGroupMessages
                .Where(m => m.GroupChatName == groupName)
                .Select(m => new HubGroupMessageMdl
                {
                    FromUserName = m.FromUserName,
                    Message = m.Message,
                    Time = m.Time,
                    AvaId = m.AvaId
                })
                .ToList();

            return messages;
        }

        public void CheckMainChat()
        {
            var main = _dbContext.hubGroups.Where(w => w.Name == "GeneralDefaultChat").Select(m => m.Name);
            if (main.FirstOrDefault() == "GeneralDefaultChat")
            {

            }
            else
            {
                var data = new HubGroup() { Name = "GeneralDefaultChat" };
                _dbContext.hubGroups.Add(data);
                _dbContext.SaveChanges();
            }
        }

        public int GetAvatarId(string Username)
        {
           int AvaId = _dbContext.Users.Where(w =>w.Username == Username).Select(u => u.AvatarId).FirstOrDefault();
            return AvaId;
        }
    }

}