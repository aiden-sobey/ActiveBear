"use strict";

class Messager {
	static AuthenticateChannel() {
		var channelKey = prompt("Please enter a channel key:", "");
		passwordInput.value = channelKey; // TODO: store this somewhere more secure than HTML DOM
		messageInput.hidden = false;
	}

	static CreateMessageBubble(message) {
		if (message === null || message === "") return;
		var decryptedText = Encryption.AesDecrypt(message.EncryptedContents);
		if (decryptedText === null) return;

		// Author box	
		var authorBubble = document.createElement("div");
		authorBubble.setAttribute('class', 'message_author');
		authorBubble.textContent = message.Sender;;
		messageContainer.appendChild(authorBubble);

		// Message box
		var messageBubble = document.createElement("div");
		messageBubble.setAttribute('class', 'message');
		messageBubble.textContent = decryptedText;
		messageContainer.appendChild(messageBubble);

		if (currentUser.innerHTML == message.Sender) {
			messageBubble.style.textAlign = "right";
            messageBubble.style.backgroundColor = "#af2dcc";
			authorBubble.style.textAlign = "right";
			authorBubble.textContent = "You";
        }

        this.UpdateScroll();
	}

	// Helpers

	static UpdateScroll() {
		messageContainer.scrollTop = messageContainer.scrollHeight;
	}
}
