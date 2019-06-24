"use strict";

class Messager {
	static AuthenticateChannel() {
		var channelKey = prompt("Please enter a channel key:", "password123");
		passwordInput.value = channelKey; // TODO: store this somewhere more secure than HTML DOM
		messageInput.hidden = false;
		ChatHub.RequestAllMessages();
	}

	static CurrentUser() {
		return document.cookie.split("User=")[1].split("; ")[0];
	}

	static CurrentChannel() {
		return window.location.href.split("/Channel/Engage/")[1].substring(0, 36);
	}

	static CreateMessageBubble(message) {
		if (message === null || message === "") return;
		var decryptedText = Encrytion.AesDecrypt(message);
		if (decryptedText === null) return;

		var messageBubble = document.createElement("div");
		messageBubble.setAttribute('class', 'message_contents');
		messageBubble.textContent = decryptedText;
		messageContainer.appendChild(messageBubble);
		UpdateScroll();
	}

	static GenerateMessagePacket(message) {
		var packet = {
			UserCookie: currentUser,
			Channel: currentChannel,
			Message: message
		}
		return JSON.stringify(packet);
	}

	static GenerateChannelPacket() {
		var packet = {
			UserCookie: currentUser,
			Channel: currentChannel
		}

		return JSON.stringify(packet);
	}

	// Helpers

	static UpdateScroll() {
		messageContainer.scrollTop = messageContainer.scrollHeight;
	}
}
