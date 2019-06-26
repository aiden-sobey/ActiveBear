"use strict";

var ChannelCreated= "ChannelCreated";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

var titleInput = document.getElementById("channel_name_submit");
var keyInput = document.getElementById("channel_key_submit");
var createButton = document.getElementById("channel_create_button");

// Send actions

keyInput.addEventListener("keydown", function(event) {
    if (event.key === "Enter")
		CreateChannel();
});

createButton.addEventListener("mousedown", function(event) {
	CreateChannel();
});

// Receive actions

connection.on(ChannelCreated, function (message) {
	if (message === null || message === "") {
		// TODO: we've got an error to handle here
	}
	else {	// We were passed a valid GUID
		var path = "/ChannelAuth/AuthUserToChannel/" + message; // TODO: constantize
		window.location.replace(path);
	}
});


/*      Connection      */


connection.start().then(function(){
	// Empty for now
}).catch(function (err) {
    return console.error(err.toString());
});


/*      Helper methods      */

function CreateChannel() {
	var title = titleInput.value;
	var key = sha256(keyInput.value);
	var packet = ChatHub.GenerateChannelCreationPacket(title, key);
	connection.invoke("CreateChannel", packet).catch(function(err) {
		return console.error(err.toString());
	});;
}
