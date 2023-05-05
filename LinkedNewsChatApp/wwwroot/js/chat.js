"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

//filled when button with online user was clicked
var toUser = null;
//filled when onconnected 
var currentUser = null;
//filled on join
var toGroup = null;
var allgroups = null;
var friends = null;
var allusers = null;

connection.on("IdentifyUser", function (user) {
    currentUser = user;

});

connection.on("AddFriendToList", function (userName) {
    addFriends({ name: userName }); // додати нового друга до списку друзів на клієнті
    document.getElementById("chat-intro").innerHTML = `Private Chat with ${userName}`;
    connection.invoke("JoinPrivateChat", userName);
});

connection.on("RemoveFriendFromList", function () {
    location.reload();
   });


connection.on("BanAllert", function (date) {
    if (date === 1) {
        alert("Вас забанено за порушення правил чату. Вам буде відновлено доступ через: " + date + " день");
    }
    else if (date > 1 && date < 5) {
        alert("Вас забанено за порушення правил чату. Вам буде відновлено доступ через: " + date + " дні");
    }
    else if (date > 4) {
        alert("Вас забанено за порушення правил чату. Вам буде відновлено доступ через: " + date + " днів");
    }
});



connection.on("RecieveOnlineFriends", function (userNames, friendslst) {
    var userNameWindow = document.getElementById("FriendOnlineList");
    var userNameWindow2 = document.getElementById("FriendOnlineList2");

    if (userNameWindow.childElementCount > 0) {

    }
    else {
        userNameWindow.innerHTML = "";
        userNameWindow2.innerHTML = "";

        if (friendslst) {
            const separator = document.createElement("li");
            separator.className = "group-separator";
            separator.textContent = "Друзі:";
            userNameWindow.appendChild(separator);
            const separator2 = separator.cloneNode(true);
            userNameWindow2.appendChild(separator2);
            friendslst.forEach(addFriends);
        }

    }
    const scrollingElement = document.getElementsByClassName("sidenavRight")[0];
    scrollingElement.scrollTop = scrollingElement.scrollHeight;
});

connection.on("RecieveOnlineUsers", function (userNames, friendslst) {
    var userNameWindow = document.getElementById("userOnlineList");
    var userNameWindow2 = document.getElementById("userOnlineList2");


    userNameWindow.innerHTML = "";
    userNameWindow2.innerHTML = "";
    const separator3 = document.createElement("li");
    separator3.className = "group-separator";
    separator3.textContent = "Юзери онлайн";
    userNameWindow.appendChild(separator3);
    const separator4 = separator3.cloneNode(true);
    userNameWindow2.appendChild(separator4);
    userNames.forEach(addOnlineUserNames);

    const scrollingElement = document.getElementsByClassName("sidenavRight")[0];
    scrollingElement.scrollTop = scrollingElement.scrollHeight;
});


connection.on("RecieveAllOnlineGroups", function (groupall, friendsList, usersall) {
    allgroups = Array.from(groupall);
    friends = Array.from(friendsList);
    allusers = Array.from(usersall);
})

function addFriends(user) {
    if (user.userIdentifier === currentUser.userIdentifier) {
        return;
    }
    var li = document.createElement("li");
    var li2 = document.createElement("li");
    var button = document.createElement("button");
    var button2 = document.createElement("button");
    // add class to style
    button.className = "user-connected";
    button.textContent = user.name;
    button.onclick = function (event) {
        //dont join if you are already in a chat
        if (toUser === user) {
            return;
        }
        document.getElementById("groupError").innerHTML = "";
        document.getElementById("groupError2").innerHTML = "";
        // change header to see with whom you chat
        document.getElementById("chat-intro").innerHTML = `Приватний чат з ${user.name}`;
        // allow to send messages and chat
        document.getElementById("sendButton").disabled = false;
        // reset button color
        button.style.backgroundColor = null;
        // user leave group chat
        if (toGroup !== null) {
            connection.invoke("LeaveRoom", toGroup);
            // clean to group
            toGroup = null;
        }
        // user leave private chat
        if (toUser !== null && toUser !== user) {
            connection.invoke("LeavePrivateChat", toUser.name);
        }
        // set user for this button
        toUser = user;
        //clean message list
        document.getElementById("messagesList").innerHTML = "";
        // add user to private chat
        connection.invoke("JoinPrivateChat", user.name);
    };

    button2.className = "user-connected";
    button2.textContent = user.name;
    button2.onclick = function (event) {
        //dont join if you are already in a chat
        if (toUser === user) {
            return;
        }
        document.getElementById("groupError").innerHTML = "";
        document.getElementById("groupError2").innerHTML = "";
        // change header to see with whom you chat
        document.getElementById("chat-intro").innerHTML = `Приватний чат з ${user.name}`;
        // allow to send messages and chat
        document.getElementById("sendButton").disabled = false;
        // reset button color
        button2.style.backgroundColor = null;
        // user leave group chat
        if (toGroup !== null) {
            connection.invoke("LeaveRoom", toGroup);
            // clean to group
            toGroup = null;
        }
        // user leave private chat
        if (toUser !== null && toUser !== user) {
            connection.invoke("LeavePrivateChat", toUser.name);
        }
        // set user for this button
        toUser = user;
        //clean message list
        document.getElementById("messagesList").innerHTML = "";
        // add user to private chat
        connection.invoke("JoinPrivateChat", user.name);
    };
    // append items
    li.appendChild(button);
    li2.appendChild(button2);
    document.getElementById("FriendOnlineList").appendChild(li);
    document.getElementById("FriendOnlineList2").appendChild(li2);
}

function addOnlineUserNames(user) {
    if (user.userIdentifier === currentUser.userIdentifier) {
        return;
    }
    var li = document.createElement("li");
    var li2 = document.createElement("li");
    var button = document.createElement("button");
    var button2 = document.createElement("button");
    // add class to style
    button.className = "user-connected";
    button.textContent = user.name;
    button.onclick = function (event) {
        //dont join if you are already in a chat
        if (toUser === user) {
            return;
        }
        document.getElementById("groupError").innerHTML = "";
        document.getElementById("groupError2").innerHTML = "";
        // change header to see with whom you chat
        document.getElementById("chat-intro").innerHTML = `Приватний чат ${user.name}`;
        // allow to send messages and chat
        document.getElementById("sendButton").disabled = false;
        // reset button color
        button.style.backgroundColor = null;
        // user leave group chat
        if (toGroup !== null) {
            connection.invoke("LeaveRoom", toGroup);
            // clean to group
            toGroup = null;
        }
        // user leave private chat
        if (toUser !== null && toUser !== user) {
            connection.invoke("LeavePrivateChat", toUser.name);
        }
        // set user for this button
        toUser = user;
        //clean message list
        document.getElementById("messagesList").innerHTML = "";
        // add user to private chat
        connection.invoke("JoinPrivateChat", user.name);
    };

    button2.className = "user-connected";
    button2.textContent = user.name;
    button2.onclick = function (event) {
        //dont join if you are already in a chat
        if (toUser === user) {
            return;
        }
        document.getElementById("groupError").innerHTML = "";
        document.getElementById("groupError2").innerHTML = "";
        // change header to see with whom you chat
        document.getElementById("chat-intro").innerHTML = `Приватний чат ${user.name}`;
        // allow to send messages and chat
        document.getElementById("sendButton").disabled = false;
        // reset button color
        button2.style.backgroundColor = null;
        // user leave group chat
        if (toGroup !== null) {
            connection.invoke("LeaveRoom", toGroup);
            // clean to group
            toGroup = null;
        }
        // user leave private chat
        if (toUser !== null && toUser !== user) {
            connection.invoke("LeavePrivateChat", toUser.name);
        }
        // set user for this button
        toUser = user;
        //clean message list
        document.getElementById("messagesList").innerHTML = "";
        // add user to private chat
        connection.invoke("JoinPrivateChat", user.name);
    };
    // append items
    li.appendChild(button);
    li2.appendChild(button2);
    document.getElementById("userOnlineList").appendChild(li);
    document.getElementById("userOnlineList2").appendChild(li2);
}

document.getElementById("emoji-button").addEventListener("click", function (event) {
    const emojiWindow = document.getElementById('emoji-window');
    if (emojiWindow.style.display === 'block') {
        emojiWindow.style.display = 'none';
    } else {
        const emojis = Array.from({ length: 80 }, (_, i) => String.fromCodePoint(0x1F600 + i));
        let emojiHTML = '';
        for (let i = 0; i < emojis.length; i++) {
            emojiHTML += '<span>' + emojis[i] + '</span>';
            if ((i + 1) % 16 === 0) {
                emojiHTML += '<br>';
            }
        }
        emojiWindow.innerHTML = '<div id="emoj">' + emojiHTML + '</div>';
        emojiWindow.style.display = 'block';
        const emojiElements = document.querySelectorAll('#emoji-window span');
        for (let i = 0; i < emojiElements.length; i++) {
            emojiElements[i].classList.add('emoji');
            emojiElements[i].addEventListener('click', function () {
                const emoji = emojiElements[i].innerHTML;
                const messageInput = document.getElementById('messageInput');
                messageInput.value += emoji;
            });
        }
    }
});







document.getElementById("sendButton").addEventListener("click", function (event) {
    var message = document.getElementById("messageInput").value;
    //send private message
    if (message !== "" && toUser !== null && toGroup === null) {
        connection.invoke("SendPrivateMessage", toUser.name, message).catch(function (err) {
            return console.error(err.toString());
        });
        $("textarea").val("");
    }
    //send message to group
    if (message !== "" && toGroup !== null && toUser === null) {
        connection.invoke("SendMessageGroup", toGroup, message).catch(function (err) {
            return console.error(err.toString());
        });
        $("textarea").val("");
    }
    event.preventDefault();
});

connection.on("MessageNotification", function (user) {
    Array.from(document.getElementsByClassName("user-connected")).forEach(function (button) {
        if (button.textContent === user.name && (toUser === null || user.name !== toUser.name)) {
            button.style.backgroundColor = "#FFA500";
        }
    });
});

connection.on("GetOldMessagesOnJoin", function (messageList) {
    Array.from(messageList);
    var messageWindow = document.getElementById("messagesList");
    messageWindow.innerHTML = "";
    messageList.forEach(addLastTenMessages);

});

function addLastTenMessages(message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);

    li.innerHTML = `<div class="one-message ${currentUser.name === message.fromUserName ? "my-message" : ""}">
<div class="break">
<a href="/Profile/UserProfile?username=${message.fromUserName}"><img src="/avatars/${message.avaId}.png"/> </a>
<p class="username"><b>${message.fromUserName}</b></p>
</div>
<div class="message">
<p class="text-content"></p>

</div>
<span class="time-right">${message.time}</span>
</div>`;

    li.getElementsByClassName("text-content")[0].textContent = message.message;
    const scrollingElement = document.getElementsByClassName("message-box")[0];
    scrollingElement.scrollTop = scrollingElement.scrollHeight;
}



connection.on("ReceiveMessage", function (user, message, timenow, avaid) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    li.innerHTML = `<div class="one-message ${currentUser.userIdentifier === user.userIdentifier ? "my-message" : ""}">
<div class="break">
<a href="/Profile/UserProfile?username=${user.name}"><img src="/avatars/${avaid}.png"/> </a>
<p class="username"><b>${user.name}</b></p>
</div>
<div class="message">
<p class="text-content"></p>

</div>
<span class="time-right">${timenow}</span>
</div>`;
    li.getElementsByClassName("text-content")[0].textContent = message;
    const scrollingElement = document.getElementsByClassName("message-box")[0];
    scrollingElement.scrollTop = scrollingElement.scrollHeight;
});

connection.on("NotifyGroup", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    li.innerHTML = `<p><center><b> ${user.name} ${message}</b></center></p>`;
    const scrollingElement = document.getElementsByClassName("message-box")[0];
    scrollingElement.scrollTop = scrollingElement.scrollHeight;
});
//validation
document.getElementById("joinGroupButton").addEventListener("click", function (event) {
    var group = document.getElementById("GroupChatName").value;
    //var groups = Array.from(document.getElementsByClassName("group-connected"));
    var groups = allgroups;
    if (groups.includes(group)) {
        var error = document.getElementById("groupError");
        error.textContent = 'Група уже існує';
        error.style = "color: red";
        $("#GroupChatName").val("");
        return;
    }
    else if (group.length > 50) {
        var error = document.getElementById("groupError");
        error.textContent = 'Наймення групи дуже довге';
        error.style = "color: red";
        $("#GroupChatName").val("");
        return;
    }
    else if (!group || group === '') {
        var error = document.getElementById("groupError");
        error.textContent = 'Введіть групу!';
        error.style = "color: red";
        $("#GroupChatName").val("");
        return;
    }
    else {
        var error = document.getElementById("groupError");
        error.textContent = 'Група успішно створена!';
        error.style = "color: green";
        connection.invoke("CreateGroup", group).catch(function (err) {
            return console.error(err.toString());
        });
    }

    $("#GroupChatName").val("");
    event.preventDefault();
});

document.getElementById("joinGroupButton2").addEventListener("click", function (event) {
    var group = document.getElementById("GroupChatName2").value;
    //var groups = Array.from(document.getElementsByClassName("group-connected"));
    var groups = allgroups;

    if (groups.includes(group)) {
        var error = document.getElementById("groupError2");
        error.textContent = 'Група уже існує';
        error.style = "color: red";
        $("#GroupChatName2").val("");
        return;
    }
    else if (group.length > 50) {
        var error = document.getElementById("groupError2");
        error.textContent = 'Наймення групи дуже довге';
        error.style = "color: red";
        $("#GroupChatName2").val("");
        return;
    }
    else if (!group || group === '') {
        var error = document.getElementById("groupError2");
        error.textContent = 'Введіть групу!';
        error.style = "color: red";
        $("#GroupChatName2").val("");
        return;
    }
    else {
        var error = document.getElementById("groupError2");
        error.textContent = 'Група успішно створена!';
        error.style = "color: green";
        connection.invoke("CreateGroup", group).catch(function (err) {
            return console.error(err.toString());
        });
    }

    $("#GroupChatName").val("");
    event.preventDefault();
});

document.getElementById("joinButton").addEventListener("click", function (event) {
    var group = document.getElementById("GroupJoin").value;
    var groups = allgroups;
    if (groups.includes(group)) {
        var error = document.getElementById("groupError");
        error.textContent = '';
        // Змінити заголовок для відображення з ким ведеться чат
        document.getElementById("chat-intro").innerHTML = `Наразі ви спілкуєтеся у групі ${group}`;
        // Встановити поточну групу
        toGroup = group;

        // Очистити поточного користувача
        toUser = null;

        // Очистити список повідомлень
        document.getElementById("messagesList").innerHTML = "";

        // Додати користувача до групового чату
        connection.invoke("JoinRoom", group);
        return;
    }
    else {
        var error = document.getElementById("groupError");
        error.textContent = 'Група не існує';
        error.style = "color: red";
        $("#GroupChatName2").val("");
        return;
    }
});


document.getElementById("joinButton2").addEventListener("click", function (event) {
    var group = document.getElementById("GroupJoin2").value;
    var groups = allgroups;
    if (groups.includes(group)) {
        var error = document.getElementById("groupError2");
        error.textContent = '';
        // Змінити заголовок для відображення з ким ведеться чат
        document.getElementById("chat-intro").innerHTML = `Наразі ви спілкуєтеся у групі ${group}`;
        // Встановити поточну групу
        toGroup = group;

        // Очистити поточного користувача
        toUser = null;

        // Очистити список повідомлень
        document.getElementById("messagesList").innerHTML = "";

        // Додати користувача до групового чату
        connection.invoke("JoinRoom", group);
        return;
    }
    else {
        var error = document.getElementById("groupError2");
        error.textContent = 'Група не існує';
        error.style = "color: red";
        $("#GroupChatName2").val("");
        return;
    }
});

document.getElementById("LeaveButton").addEventListener("click", function (event) {
    connection.invoke("LeaveFromGroup", toGroup);
    return;
});

document.getElementById("FriendButton").addEventListener("click", function (event) {
    var friend = document.getElementById("FriendJoin").value;
    connection.invoke("FriendValid", friend).catch(function (err) {
        return console.error(err.toString());
    });
    //connection.invoke("AddFriend", user);
    //return;
});
document.getElementById("FriendButton2").addEventListener("click", function (event) {
    var friend = document.getElementById("FriendJoin2").value;
    connection.invoke("FriendValid2", friend).catch(function (err) {
        return console.error(err.toString());
    });
    //connection.invoke("AddFriend", user);
    //return;
});


connection.on("addfriendjs", function (friend, users, friends) {
    var allfriends = Array.from(friends);
    var fusers = Array.from(users);

    if (fusers.includes(friend)) {

        if (allfriends.includes(friend)) {
            var error = document.getElementById("userError")
            error.textContent = 'Друг уже добавлений!';
            error.style = "color: red";
            $("#FriendJoin").val("");
            return;
        }
        else if (friend.length > 50) {
            var error = document.getElementById("userError");
            error.textContent = 'Наймення друга дуже довге';
            error.style = "color: red";
            $("#FriendJoin").val("");
            return;
        }
        else if (!friend || friend === '') {
            var error = document.getElementById("userError");
            error.textContent = 'Введіть друга!';
            error.style = "color: red";
            $("#FriendJoin").val("");
            return;
        }
        else {
            var error = document.getElementById("userError");
            error.textContent = 'Друг успішно доданий!';
            error.style = "color: green";
            connection.invoke("AddFriend", friend).catch(function (err) {
                return console.error(err.toString());
            });
        }
    }
    else {
        var error = document.getElementById("userError")
        error.textContent = 'Користувач не існує!';
        error.style = "color: red";
        $("#FriendJoin").val("");
        return;
    }
    $("#FriendJoin").val("");
});
connection.on("addfriendjs2", function (friend, users, friends) {
    var allfriends = Array.from(friends);
    var fusers = Array.from(users);
    if (fusers.includes(friend)) {
        if (allfriends.includes(friend)) {
            var error = document.getElementById("userError2")
            error.textContent = 'Друг уже добавлений!';
            error.style = "color: red";
            $("#FriendJoin2").val("");
            return;
        }
        else if (friend.length > 50) {
            var error = document.getElementById("userError2");
            error.textContent = 'Наймення друга дуже довге';
            error.style = "color: red";
            $("#FriendJoin2").val("");
            return;
        }
        else if (!friend || friend === '') {
            var error = document.getElementById("userError2");
            error.textContent = 'Введіть друга!';
            error.style = "color: red";
            $("#FriendJoin2").val("");
            return;
        }
        else {
            var error = document.getElementById("userError2");
            error.textContent = 'Друг успішно доданий!';
            error.style = "color: green";
            connection.invoke("AddFriend", friend).catch(function (err) {
                return console.error(err.toString());
            });
        }
    }
    else {
        var error = document.getElementById("userError2")
        error.textContent = 'Користувач не існує!';
        error.style = "color: red";
        $("#FriendJoin2").val("");
        return;
    }
    $("#FriendJoin2").val("");
});
connection.on("deletefriendjs", function (friend, friends) {
    var allfriends = Array.from(friends);
    if (allfriends.includes(friend)) {
        var error = document.getElementById("userError");
        error.textContent = 'Друг успішно видалений!';
        error.style = "color: green";
        connection.invoke("DeleteFriend", friend).catch(function (err) {
            return console.error(err.toString());
        });
    }
    else {
        var error = document.getElementById("userError")
        error.textContent = 'Друга не існує!';
        error.style = "color: red";
        $("#FriendDelete").val("");
        return;
    }
    $("#FriendDelete").val("");
});
connection.on("deletefriendjs2", function (friend, friends) {
    var allfriends = Array.from(friends);
    if (allfriends.includes(friend)) {
        var error = document.getElementById("userError2");
        error.textContent = 'Друг успішно видалений!';
        error.style = "color: green";
        connection.invoke("DeleteFriend", friend).catch(function (err) {
            return console.error(err.toString());
        });
    }
    else {
        var error = document.getElementById("userError2")
        error.textContent = 'Друга не існує!';
        error.style = "color: red";
        $("#FriendDelete2").val("");
        return;
    }
    $("#FriendDelete2").val("");
});


document.getElementById("FriendRemove").addEventListener("click", function (event) {
    var friend = document.getElementById("FriendDelete").value;
    connection.invoke("FriendDeleteValid", friend).catch(function (err) {
        return console.error(err.toString());
    });
    //connection.invoke("AddFriend", user);
    //return;
});

document.getElementById("FriendRemove2").addEventListener("click", function (event) {
    var friend = document.getElementById("FriendDelete2").value;
    connection.invoke("FriendDeleteValid2", friend).catch(function (err) {
        return console.error(err.toString());
    });
    //connection.invoke("AddFriend", user);
    //return;
});

connection.on("AddCreatorToGroup", function (user, groupName) {
    // change header to see with whom you chat
    document.getElementById("chat-intro").innerHTML = `Наразі ви спілкуєтеся у групі ${group}`;
    // allow to send messages and chat
    document.getElementById("sendButton").disabled = false;
    // user leave group chat
    if (toGroup !== null && toGroup !== groupName) {
        connection.invoke("LeaveRoom", toGroup);
    }
    // user leave private chat
    if (toUser !== null) {
        connection.invoke("LeavePrivateChat", toUser.name);
        // clean toUser
        toUser = null;
    }
    // set group for this button
    toGroup = groupName;
    //clean message list
    document.getElementById("messagesList").innerHTML = "";
});


connection.on("AddToMainChat", function (user, groupName) {
    // change header to see with whom you chat
    document.getElementById("chat-intro").innerHTML = `Наразі ви спілкуєтеся у загальному чаті`;
    // allow to send messages and chat
    document.getElementById("sendButton").disabled = false;
    // user leave group chat
    if (toGroup !== null && toGroup !== groupName) {
        connection.invoke("LeaveRoom", toGroup);
    }
    // user leave private chat
    if (toUser !== null) {
        connection.invoke("LeavePrivateChat", toUser.name);
        // clean toUser
        toUser = null;
    }
    // set group for this button
    toGroup = groupName;
    //clean message list
    document.getElementById("messagesList").innerHTML = "";
});

const regionsOfUkraine = [
    "АР Крим",
    "Вінницька область",
    "Волинська область",
    "Дніпропетровська область",
    "Донецька область",
    "Житомирська область",
    "Закарпатська область",
    "Запорізька область",
    "Івано-Франківська область",
    "Київська область",
    "Кіровоградська область",
    "Луганська область",
    "Львівська область",
    "Миколаївська область",
    "Одеська область",
    "Полтавська область",
    "Рівненська область",
    "Сумська область",
    "Тернопільська область",
    "Харківська область",
    "Херсонська область",
    "Хмельницька область",
    "Черкаська область",
    "Чернівецька область",
    "Чернігівська область",
];

// receiveing groups online
connection.on("RecieveOnlineGroups", function (groupNames, superMainGroup, user) {
    Array.from(groupNames);
    var groupNameWindow = document.getElementById("groupList");
    var groupNameWindow2 = document.getElementById("groupList2");
    const groupList1 = document.getElementById("groupList");
    const groupList2 = document.getElementById("groupList2");
    const buttons = groupList1.getElementsByTagName('button');
    const buttons2 = groupList2.getElementsByTagName('button');

    if (user.userIdentifier !== currentUser.userIdentifier) {
        if (groupList1.childElementCount > 0) {
            if ((buttons[0].textContent !== null && buttons[0].textContent !== '') && (buttons2[0].textContent !== null && buttons2[0].textContent !== '')) {
                superMainGroup = buttons[0].textContent;
            }
        }
    }
    else {
        groupNameWindow2.innerHTML = "";
        groupNameWindow.innerHTML = "";
        const sup = [superMainGroup]
        // add supermaingroup to the beginning
        if (superMainGroup) {
            const separator = document.createElement("li");
            separator.className = "group-separator";
            separator.textContent = "Ваш місцевий чат";
            groupNameWindow.appendChild(separator);
            const separator2 = separator.cloneNode(true);
            groupNameWindow2.appendChild(separator2);
            sup.forEach(addOnlineGroupNames);
        }

        // create an array of non-main groups
        const nonMainGroups = groupNames.filter(function (group) {
            return !regionsOfUkraine.includes(group);
        });

        // loop through regionsOfUkraine and add main groups
        if (regionsOfUkraine.length > 0) {
            const separator = document.createElement("li");
            separator.className = "group-separator";
            separator.textContent = "Чати регіонів";
            groupNameWindow.appendChild(separator);
            const separator2 = separator.cloneNode(true);
            groupNameWindow2.appendChild(separator2);
            regionsOfUkraine.forEach(function (region) {
                const groupsInRegion = groupNames.filter(function (group) {
                    return group.includes(region);
                });
                groupsInRegion.forEach(addOnlineGroupNames);
            });
        }

        // add non-main groups to the end
        if (nonMainGroups.length > 0) {
            const separator = document.createElement("li");
            separator.className = "group-separator";
            separator.textContent = "Інші групи";
            groupNameWindow.appendChild(separator);
            const separator2 = separator.cloneNode(true);
            groupNameWindow2.appendChild(separator2);
            nonMainGroups.forEach(addOnlineGroupNames);
        }
    }


    const scrollingElement = document.getElementsByClassName("groups-main")[0];
    scrollingElement.scrollTop = scrollingElement.scrollHeight;
});


function addOnlineGroupNames(group) {
    const li = document.createElement("li");
    const li2 = document.createElement("li"); // create new li element for groupList2
    const button = document.createElement("button");
    const button2 = document.createElement("button");

    // Додати клас, щоб візуально стилізувати кнопки

    button.className = "group-connected";
    button.textContent = group;
    button.onclick = handleGroupButtonClick;

    button2.className = "group-connected";
    button2.textContent = group;
    button2.onclick = handleGroupButtonClick;

    // Додати кнопку до списку груп
    li.appendChild(button);
    li2.appendChild(button2); // append cloned button to new li element
    document.getElementById("groupList").appendChild(li);
    document.getElementById("groupList2").appendChild(li2);
}

function handleGroupButtonClick(event) {
    const clickedButton = event.target;
    const groupName = clickedButton.textContent;

    // Не додавати користувача до чату, якщо він вже там знаходиться
    if (toGroup === groupName) {
        return;
    }

    // Очистити повідомлення про помилку, якщо воно було раніше
    document.getElementById("groupError").innerHTML = "";
    document.getElementById("groupError2").innerHTML = "";

    // Змінити заголовок для відображення з ким ведеться чат
    document.getElementById("chat-intro").innerHTML = `Наразі ви спілкуєтеся у групі ${groupName}`;

    // Увімкнути кнопку відправки повідомлень
    document.getElementById("sendButton").disabled = false;

    // Якщо користувач вже знаходиться в чаті, то він спочатку повинен вийти з нього
    if (toGroup !== null && toGroup !== groupName) {
        connection.invoke("LeaveRoom", toGroup);
    }

    // Якщо користувач знаходиться в приватному чаті, то він спочатку повинен вийти з нього
    if (toUser !== null) {
        connection.invoke("LeavePrivateChat", toUser.name);
    }

    // Встановити поточну групу
    toGroup = groupName;

    // Очистити поточного користувача
    toUser = null;

    // Очистити список повідомлень
    document.getElementById("messagesList").innerHTML = "";

    // Додати користувача до групового чату
    connection.invoke("JoinRoom", groupName);
}


connection.start()
    .then(function () {
        document.getElementById("sendButton").disabled;
    })
    .catch(function (err) {
        //alert("Ваш сеанс завершився, оновити сторінку?");
        //location.reload();
        console.error(err.toString());
    });


let isLeavingPage = false;

window.addEventListener('beforeunload', function (e) {
    // користувач переходить на іншу сторінку
    isLeavingPage = true;
});

connection.onclose(function (e) {
    
        if (!isLeavingPage && document.readyState === "complete") {
            // видаємо alert, якщо користувач не переходить на іншу сторінку
            alert("Ваш сеанс завершився, оновити сторінку?");
            location.reload();
        }
   
});

