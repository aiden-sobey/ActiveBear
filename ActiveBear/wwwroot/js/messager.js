"use strict";

class Messager {
	static AuthenticateChannel() {
		var channelKey = prompt("Please enter a channel key:", "");
		passwordInput.value = channelKey; // TODO: store this somewhere more secure than HTML DOM
		messageInput.hidden = false;
	}

	static CreateMessageBubble(message) {
		if (message === null || message === "") return;
		var decryptedText = Encryption.AesDecrypt(message);
		if (decryptedText === null) return;

		var messageBubble = document.createElement("div");
		messageBubble.setAttribute('class', 'message_contents');
		messageBubble.textContent = decryptedText;
		messageContainer.appendChild(messageBubble);
		this.UpdateScroll();
	}

	// Helpers

	static UpdateScroll() {
		messageContainer.scrollTop = messageContainer.scrollHeight;
	}
}
