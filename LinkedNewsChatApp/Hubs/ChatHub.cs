using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using ServiceLayer;
using System.Timers;
using DataLayer;
using System.Text.RegularExpressions;

namespace LinkedNewsChatApp.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        public static List<HubUser> connectedUsers = new List<HubUser>();
        private static MessageDictionary _messageDictionary = new MessageDictionary();
        private static GroupDictionary _groupDictionary = new GroupDictionary();
        private readonly LinkedNewsRepository _repository;
        private readonly LoginOperations _loginOperator;
        public ChatHub(LinkedNewsRepository repository, LoginOperations loginOperator)
        {
            _repository = repository;
            _loginOperator = loginOperator;

        }

        public override async Task OnConnectedAsync()
        {
            var hubUser = new HubUser()
            {
                UserIdentifier = Context.UserIdentifier,
                Name = Context.User.Identity.Name

            };

            //lock this list while completing method
            lock (connectedUsers)
            {
                if (connectedUsers.Find(x => x.Name == hubUser.Name) == null)
                {
                    connectedUsers.Add(hubUser);
                }
            }
            //---------------------
            var chatOperations = new ChatOperations(_repository, _loginOperator);
            //---------------------
            chatOperations.CheckMainChat();
            string reg = chatOperations.GetReg(hubUser.Name);

            await Clients.Caller.SendAsync("IdentifyUser", hubUser);
            await Clients.All.SendAsync("RecieveOnlineUsers", connectedUsers);
            await Clients.All.SendAsync("RecieveAllOnlineGroups", chatOperations.GetAllListOfGroups());
            await Clients.All.SendAsync("RecieveOnlineGroups", chatOperations.GetListOfGroups(hubUser.Name), reg); //chatOperations.GetListOfGroups());
            //add user to general chat
            var generalChatName = "GeneralDefaultChat";
            await Groups.AddToGroupAsync(Context.ConnectionId, generalChatName);
            await Clients.User(hubUser.UserIdentifier).SendAsync("AddToMainChat", hubUser, generalChatName);

            //зчитування повідомлень зі словника
            var messageList1 = _messageDictionary.GetLastMessageList(generalChatName);
            //   int AvaId = chatOperations.AvaId(Username);
            var messageList = chatOperations.GetGroupMessages(generalChatName);
            if (messageList != null)
            {
                await Clients.User(hubUser.UserIdentifier).SendAsync("GetOldMessagesOnJoin", messageList);
            }
            //кінець
            await Clients.Group(generalChatName).SendAsync("NotifyGroup", hubUser, " joined General Chat").ConfigureAwait(true);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            //---------------------
            var chatOperations = new ChatOperations(_repository, _loginOperator);
            //---------------------
            var hubUser = connectedUsers.Find(x => x.UserIdentifier == Context.UserIdentifier);
            lock (connectedUsers)
            {
                if (hubUser != null)
                {
                    connectedUsers.Remove(hubUser);
                }
            }
            string reg = chatOperations.GetReg(hubUser.Name);
            _groupDictionary.RemoveUserFromGroup(hubUser);
            await Clients.All.SendAsync("RecieveAllOnlineGroups", chatOperations.GetAllListOfGroups());
            await Clients.All.SendAsync("RecieveOnlineGroups", chatOperations.GetListOfGroups(hubUser.Name), reg);
            await Clients.All.SendAsync("RecieveOnlineUsers", connectedUsers);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task JoinPrivateChat(string toUser)
        {
            var hubUser = new HubUser()
            {
                UserIdentifier = Context.UserIdentifier,
                Name = Context.User.Identity.Name
            };

            var groupName = CreatePrivateGroupName(hubUser.Name, toUser);
            //var messageList = _messageDictionary.GetLastMessageList(groupName);
            var chatOperations = new ChatOperations(_repository, _loginOperator);
            var messageList = chatOperations.GetPrivateMessages(groupName);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            int Avaid = chatOperations.AvaId(hubUser.Name);
            if (messageList != null)
            {
                await Clients.User(hubUser.UserIdentifier).SendAsync("GetOldMessagesOnJoin", messageList, Avaid);
            }
            await Clients.Group(groupName).SendAsync("NotifyGroup", hubUser, " is here! ").ConfigureAwait(true);
        }

        public async Task SendPrivateMessage(string toUser, string message)
        {
            var hubUser = new HubUser()
            {
                UserIdentifier = Context.UserIdentifier,
                Name = Context.User.Identity.Name
            };

            //find user in a list to get user identifier 
            var foundToUser = connectedUsers.FirstOrDefault(x => x.Name == toUser);
            //time when message was sent
            var timeNow = DateTime.Now;
            var groupName = CreatePrivateGroupName(hubUser.Name, toUser);
            var chatOperations = new ChatOperations(_repository, _loginOperator);
            chatOperations.AddChat(groupName);
            var saveMessage = new HubMessage()
            {
                FromUserName = hubUser.Name,
                Time = timeNow.ToString("HH:mm:ss"),
                Message = message
            };
            //------------------
            string FromUserName = hubUser.Name;
            string Time = timeNow.ToString("HH:mm:ss");
            string Message = message;
            string groupname = groupName;

            int Avaid = chatOperations.AvaId(hubUser.Name);
            chatOperations.AddPrivateMessage(FromUserName, Time, Message, groupname, Avaid);
            //------------------
            //_messageDictionary.Add(groupName, saveMessage);

            await Clients.Group(groupName).SendAsync("ReceiveMessage", hubUser, message, timeNow.ToString("HH:mm:ss"), Avaid);
            //send notification to user
            await Clients.User(foundToUser.UserIdentifier).SendAsync("MessageNotification", hubUser);
        }

        public string CreatePrivateGroupName(string userFrom, string userTo)
        {
            //compare two usernames to make same chat naming for both users
            var compareNames = string.Compare(userFrom, userTo);
            if (compareNames > 0)
            {
                var groupName = $"{userFrom}-{userTo}";
                return groupName;
            }
            else
            {
                var groupName = $"{userTo}-{userFrom}";
                return groupName;
            }
        }

        public async Task SendMessageGroup(string toGroup, string message)
        {
            var hubUser = new HubUser()
            {
                UserIdentifier = Context.UserIdentifier,
                Name = Context.User.Identity.Name
            };

            //time when message was sent
            var timeNow = DateTime.Now;
            //добавив зберігання повідомлень з особистих груп у словник 
            var saveMessage = new HubMessage()
            {
                FromUserName = hubUser.Name,
                Time = timeNow.ToString("HH:mm:ss"),
                Message = message
            };
            //------------
            string FromUserName = hubUser.Name;
            string Time = timeNow.ToString("HH:mm:ss");
            string Message = message;
            string groupname = toGroup;

            var chatOperations = new ChatOperations(_repository, _loginOperator);
            int Avaid = chatOperations.AvaId(hubUser.Name);
            chatOperations.AddGroupMessage(FromUserName, Time, Message, groupname, Avaid);


            //-----------
            //_messageDictionary.Add(toGroup, saveMessage);
            //кінець
            await Clients.Group(toGroup).SendAsync("ReceiveMessage", hubUser, message, timeNow.ToString("HH:mm:ss"), Avaid);
        }

        public async Task CreateGroup(string groupName)
        {
            var hubUser = new HubUser()
            {
                UserIdentifier = Context.UserIdentifier,
                Name = Context.User.Identity.Name
            };
            //add to dictionary
            //_groupDictionary.Add(groupName, hubUser);
            //-----------------------
            var UserIdentifier = Context.UserIdentifier;
            var Name = Context.User.Identity.Name;
            var chatOperations = new ChatOperations(_repository, _loginOperator);
            chatOperations.AddGroup(groupName, UserIdentifier, Name);

            //-----------------------
            string reg = chatOperations.GetReg(hubUser.Name);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.All.SendAsync("RecieveAllOnlineGroups", chatOperations.GetAllListOfGroups());
            await Clients.All.SendAsync("RecieveOnlineGroups", chatOperations.GetListOfGroups(hubUser.Name), reg);
            //add him immediatly to this chat
            await Clients.User(hubUser.UserIdentifier).SendAsync("AddCreatorToGroup", hubUser, groupName);
        }

        public async Task JoinRoom(string groupName)
        {
            //---------------------
            var chatOperations = new ChatOperations(_repository, _loginOperator);
            //---------------------
            var hubUser = new HubUser()
            {
                UserIdentifier = Context.UserIdentifier,
                Name = Context.User.Identity.Name
            };
            var UserIdentifier = Context.UserIdentifier;
            var Name = Context.User.Identity.Name;
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            //_groupDictionary.Add(groupName, hubUser);
            chatOperations.addMember(groupName, UserIdentifier, Name);
            string reg = chatOperations.GetReg(hubUser.Name);
            await Clients.All.SendAsync("RecieveAllOnlineGroups", chatOperations.GetAllListOfGroups());
            await Clients.All.SendAsync("RecieveOnlineGroups", chatOperations.GetListOfGroups(Name), reg);

            //апдейт: для особистих груп добавив читання історії повідомлень
            //var messageList = _messageDictionary.GetLastMessageList(groupName);
            var messageList = chatOperations.GetGroupMessages(groupName);
            if (messageList != null)
            {
                await Clients.User(hubUser.UserIdentifier).SendAsync("GetOldMessagesOnJoin", messageList);
            }
            //кінець 
            await Clients.Group(groupName).SendAsync("NotifyGroup", hubUser, " joined " + groupName).ConfigureAwait(true);
        }

        public async Task LeaveRoom(string groupName)
        {
            //---------------------
            var chatOperations = new ChatOperations(_repository, _loginOperator);
            //---------------------
            var hubUser = new HubUser()
            {
                UserIdentifier = Context.UserIdentifier,
                Name = Context.User.Identity.Name
            };

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            //remove from dictionary
            _groupDictionary.Remove(groupName, hubUser);
            string reg = chatOperations.GetReg(hubUser.Name);
            await Clients.All.SendAsync("RecieveOnlineGroups", chatOperations.GetListOfGroups(hubUser.Name), reg);
            if (groupName == "GeneralDefaultChat")
            {
                await Clients.Group(groupName).SendAsync("NotifyGroup", hubUser, " left General Chat").ConfigureAwait(true);
            }
            else
            {
                await Clients.Group(groupName).SendAsync("NotifyGroup", hubUser, " left " + groupName).ConfigureAwait(true);
            }
        }

        public async Task LeavePrivateChat(string toUserName)
        {
            var hubUser = new HubUser()
            {
                UserIdentifier = Context.UserIdentifier,
                Name = Context.User.Identity.Name
            };

            var groupName = CreatePrivateGroupName(hubUser.Name, toUserName);

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("NotifyGroup", hubUser, " left private chat! Bye, Bye! ").ConfigureAwait(true);
        }
        public async Task LeaveFromGroup(string group)
        {
            var hubUser = new HubUser()
            {
                UserIdentifier = Context.UserIdentifier,
                Name = Context.User.Identity.Name
            };
            var chatOperations = new ChatOperations(_repository, _loginOperator);
            chatOperations.LeaveFromGroup(hubUser.Name,group);
            await OnConnectedAsync();
        }
    }
}

