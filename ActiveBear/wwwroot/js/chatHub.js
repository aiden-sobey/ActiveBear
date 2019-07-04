"use strict";

var SendMessage = "SendMessage";
var GetChannelMessages = "GetChannelMessages";

class ChatHub {
	static PostMessage(message) {
		if (message === null || message === "") return;
		if (passwordInput.value === "" || passwordInput.value === null)
			return; // TODO: throw password empty error here

		var message = Encryption.AesEncrypt(messageInput.value);
		var messagePacket = this.GenerateMessagePacket(message);
		connection.invoke(SendMessage, messagePacket).catch(function (err) {
			return console.error(err.toString());
		});
		messageInput.value = "";
    }

    static GetCurrentUser() {
        connection.invoke(CurrentUser, this.GenerateCookiePacket()).catch(function (err) {
            return console.error(err.toString());
        })
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

	static CurrentUserCookie() {
		return document.cookie.split("User=")[1].split("; ")[0];
	}

	static CurrentChannel() {
		return window.location.href.split("/Channel/Engage/")[1].substring(0, 36);
	}

	// Communication packets

	static GenerateMessagePacket(message) {
		var packet = {
			UserCookie: this.CurrentUserCookie(),
			Channel: this.CurrentChannel(),
			Message: message
		}
		return JSON.stringify(packet);
    }

    static GenerateCookiePacket() {
        var packet = {
            UserCookie: this.CurrentUserCookie(),
        }
        return JSON.stringify(packet);
    }

	static GenerateChannelPacket() {
		var packet = {
			UserCookie: this.CurrentUserCookie(),
			Channel: this.CurrentChannel()
		}

		return JSON.stringify(packet);
	}

	static GenerateChannelCreationPacket(title, key) {
		var packet = {
			UserCookie: this.CurrentUserCookie(),
			ChannelTitle: title,
			ChannelKey: key
		}

		return JSON.stringify(packet);
	}
}
