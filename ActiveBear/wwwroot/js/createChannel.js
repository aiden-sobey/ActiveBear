"use strict";

var BuildErrors = "BuildErrors";
var ChannelCreated= "ChannelCreated";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

var nameInput = document.getElementById("channel_name_input");
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

connection.on(BuildErrors, function (message) {
	// TODO
});

connection.on(ChannelCreated, function (message) {
	// TODO
});


/*      Connection      */


connection.start().then(function(){
	// Empty for now
}).catch(function (err) {
    return console.error(err.toString());
});


/*      Helper methods      */

function CreateChannel() {
	// TODO
}
