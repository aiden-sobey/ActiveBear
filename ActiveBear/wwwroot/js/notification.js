"use strict";

// Methods invokable by SignalR
var CurrentUser = "CurrentUser";
var Notification = "Notification";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

var currentUser = document.getElementById("currentUser");
var channelDisplay = document.getElementById("channel_display");
var passwordInput = document.getElementById("passwordInput");
var messageContainer = document.getElementById("container");
var body = document.getElementById("body");
var navBar = document.getElementById("navbar");
var bodyContent = document.getElementsByClassName('body-content')[0];

// Send actions

// Receive actions

connection.on(Notification, function (notificationBlob) {
	var notification = JSON.parse(messageBlob);
	CreateNotification(notification.Contents);
});

connection.on(CurrentUser, function (user) {
  var div = document.createElement('div')
  div.innerHTML = "<div id='error_area'><div id='error'>Test!</div></div>"
  bodyContent.insertBefore(div, channelDisplay)
});

// Connection

connection.start().then(function(){
	// Dynamically set chat height
	ChatHub.GetCurrentUser();
  ChatHub.DeleteChannel();

}).catch(function (err) {
	return console.error(err.toString());
});
