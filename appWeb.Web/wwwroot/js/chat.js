"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " says " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});
//class Message {
//    constructor(username, text, date) {
//        this.userName = username;
//        this.text = text;
//        this.date = date;
//    }
//}

//// userName is declared in razor page.
//const username = userName;
//const textInput = document.getElementById('messageText');
//const whenInput = document.getElementById('date');
//const chat = document.getElementById('chat');
//const messagesQueue = [];

//document.getElementById('submitButton').addEventListener('click', () => {
//    var currentdate = new Date();
//    date.innerHTML =
//        (currentdate.getMonth() + 1) + "/"
//        + currentdate.getDate() + "/"
//        + currentdate.getFullYear() + " "
//        + currentdate.toLocaleString('en-US', { hour: 'numeric', minute: 'numeric', hour12: true })
//});

//function clearInputField() {
//    messagesQueue.push(textInput.value);
//    textInput.value = "";
//}

//function sendMessage() {
//    let text = messagesQueue.shift() || "";
//    if (text.trim() === "") return;
    
//    let date = new Date();
//    let message = new Message(username, text);
//    sendMessageToHub(message);
//}

//function addMessageToChat(message) {
//    let isCurrentUserMessage = message.userName === username;

//    let container = document.createElement('div');
//    container.className = isCurrentUserMessage ? "container darker" : "container";

//    let sender = document.createElement('p');
//    sender.className = "sender";
//    sender.innerHTML = message.userName;
//    let text = document.createElement('p');
//    text.innerHTML = message.text;

//    let date = document.createElement('span');
//    date.className = isCurrentUserMessage ? "time-left" : "time-right";
//    var currentdate = new Date();
//    date.innerHTML = 
//        (currentdate.getMonth() + 1) + "/"
//        + currentdate.getDate() + "/"
//        + currentdate.getFullYear() + " "
//        + currentdate.toLocaleString('en-US', { hour: 'numeric', minute: 'numeric', hour12: true })

//    container.appendChild(sender);
//    container.appendChild(text);
//    container.appendChild(date);
//    chat.appendChild(container);
//}
