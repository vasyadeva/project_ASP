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
            var valid = _dbContext.hubUsers.Where(m => m.GroupChatName == user.GroupChatName).Select(g => g.GroupChatName).FirstOrDefault();
            if (valid == null)
            {
                _dbContext.hubUsers.Add(user);
                _dbContext.SaveChanges();
            }
        }
        public List<string> GetListOfGroups(string name)
        {
            var usergroups = _dbContext.hubUsers.Where(w => w.Name == name).Select(h => h.GroupChatName).ToList();
            var groupList = _dbContext.hubGroups.Where(k => usergroups.Contains(k.Name)).Where(m => m.Name != "GeneralDefaultChat").Select(g => g.Name).ToList();
            return groupList;
        }
        public List<string> GetAllListOfGroups()
        {
            var groupList = _dbContext.hubGroups.Where(m => m.Name != "GeneralDefaultChat").Select(g => g.Name).ToList();
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
            string[] regionsOfUkraine = { "GeneralDefaultChat","Вінницька область", "Волинська область", "Дніпропетровська область", "Донецька область", "Житомирська область", "Закарпатська область", "Запорізька область", "Івано-Франківська область", "Київська область", "Кіровоградська область", "Луганська область", "Львівська область", "Миколаївська область", "Одеська область", "Полтавська область", "Рівненська область", "Сумська область", "Тернопільська область", "Харківська область", "Херсонська область", "Хмельницька область", "Черкаська область", "Чернівецька область", "Чернігівська область" };
            foreach (string reg in regionsOfUkraine)
            {
                var main = _dbContext.hubGroups.Where(w => w.Name == reg).Select(m => m.Name);
                if (main.FirstOrDefault() == reg)
                {

                }
                else
                {
                    var data = new HubGroup() { Name = reg };
                    _dbContext.hubGroups.Add(data);
                    _dbContext.SaveChanges();
                }
            }
        }

        public int GetAvatarId(string Username)
        {
           int AvaId = _dbContext.Users.Where(w =>w.Username == Username).Select(u => u.AvatarId).FirstOrDefault();
            return AvaId;
        }
        public string Getregion(string Username)
        {
            string reg = _dbContext.Users.Where(w => w.Username == Username).Select(u => u.Region).FirstOrDefault();
            return reg;
        }

            public void AddPrivateGroup(PrivateChat group)
            {
                string chat = _dbContext.privateChats.Where(w => w.Name == group.Name).Select(u => u.Name).FirstOrDefault();
                if (chat == null)
                {
                    _dbContext.privateChats.Add(group);
                    _dbContext.SaveChanges();
                }
            }

        public void LeaveFromGroup(string username, string group)
        {
            var user = _dbContext.hubUsers.Where(g => g.Name == username).Where(k=>k.GroupChatName==group).FirstOrDefault();
            if (user != null)
            {
                _dbContext.hubUsers.Remove(user);
                _dbContext.SaveChanges();
            }
        }
    }

}