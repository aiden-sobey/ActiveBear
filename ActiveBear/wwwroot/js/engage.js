"use strict";

// Methods invokable by SignalR
var ReceiveMessage = "ReceiveMessage";
var ReceiveAllMessages = "ReceiveAllMessages";
var CurrentUser = "CurrentUser";
var Notification = "Notification";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

var currentUser = document.getElementById("currentUser"); // TODO: dont use DOM for this
var messageInput = document.getElementById("messageInput");
var passwordInput = document.getElementById("passwordInput");
var messageContainer = document.getElementById("container");
var body = document.getElementById("body");

// Send actions

messageInput.addEventListener("keydown", function(event) {
	if (event.key === "Enter")
		ChatHub.PostMessage();
});

// Receive actions

connection.on(ReceiveMessage, function (messageBlob) {
	var message = JSON.parse(messageBlob);
	Messager.CreateMessageBubble(message);
});

connection.on(CurrentUser, function (user) {
    currentUser.innerHTML = user;
});

connection.on(ReceiveAllMessages, function (messageList) {
	var messages = JSON.parse(messageList);
	messages.forEach(function(message) {
		Messager.CreateMessageBubble(message);
	});
});

connection.on(Notification, function(message) {
	Messager.Notification(message);
});

// Connection

connection.start().then(function(){
	Messager.AuthenticateChannel();
	ChatHub.GetCurrentUser();

	// Dynamically set chat height
	var navHeight = $("#navbar").height();
	var height = window.innerHeight - navHeight - 50 + "px";
	var width = Math.round(screen.width * 0.9) + "px";

	messageContainer.style.height = height;
	body.style.width = width;
	messageContainer.style.width = width;
	messageInput.style.width = width;

	ChatHub.RequestAllMessages();

}).catch(function (err) {
	return console.error(err.toString());
});
