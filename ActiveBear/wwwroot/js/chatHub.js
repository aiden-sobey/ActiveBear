"use strict";

var SendMessage = "SendMessage";
var GetChannelMessages = "GetChannelMessages";

class ChatHub {
	static PostMessage(message) {
		if (message === null || message === "") return;
		if (passwordInput.value === "" || passwordInput.value === null)
			return PasswordEmptyError();

		var message = Encryption.AesEncrypt(messageInput.value);
		var messagePacket = this.GenerateMessagePacket(message);
		connection.invoke(SendMessage, messagePacket).catch(function (err) {
			return console.error(err.toString());
		});
		messageInput.value = "";
	}

	static RequestAllMessages() {
		connection.invoke(GetChannelMessages, this.GenerateChannelPacket()).catch(function (err) {
			return console.error(err.toString());
		});
	}

	static RemoveAllMessages() {
		while (messageContainer.firstChild) {
			messageContainer.removeChild(messageContainer.firstChild);
		}
	}

	// Runtime values

	static CurrentUser() {
		return document.cookie.split("User=")[1].split("; ")[0];
	}

	static CurrentChannel() {
		return window.location.href.split("/Channel/Engage/")[1].substring(0, 36);
	}

	// Communication packets

	static GenerateMessagePacket(message) {
		var packet = {
			UserCookie: this.CurrentUser(),
			Channel: this.CurrentChannel(),
			Message: message
		}
		return JSON.stringify(packet);
	}

	static GenerateChannelPacket() {
		var packet = {
			UserCookie: this.CurrentUser(),
			Channel: this.CurrentChannel()
		}

		return JSON.stringify(packet);
	}

	static GenerateChannelCreationPacket(title, key) {
		var packet = {
			UserCookie: this.CurrentUser(),
			ChannelTitle: title,
			ChannelKey: key
		}

		return JSON.stringify(packet);
	}
}
