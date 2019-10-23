"use strict";

class Messager {
	constructor(messageHolder, passwordHolder, messageContainer) {
		this.lastMessageAuthor = "";
		this.messageHolder = messageHolder;
		this.passwordHolder = passwordHolder;
		this.messageContainer = messageContainer;
		this.AuthenticateChannel();
	}

	AuthenticateChannel() {
		var channelKey = prompt("Please enter a channel key:", "");
		this.passwordHolder.value = channelKey; // TODO: store this somewhere more secure than HTML DOM
		this.messageHolder.hidden = false;
	}

	CreateMessageBubble(message) {
		if (message === null || message === "") return;
		var decryptedText = Encryption.AesDecrypt(message.EncryptedContents);
		if (decryptedText === null) return;

		var senderIsCurrentUser = (currentUser.innerHTML === message.Sender);
		var sameSenderAsLastMessage = (this.lastMessageAuthor === message.Sender);

		// Author box
		if (!sameSenderAsLastMessage) {
			var authorBubble = document.createElement("div");
			authorBubble.setAttribute('class', 'message_author');
			if (senderIsCurrentUser) {
				authorBubble.textContent = "You";
				authorBubble.style.textAlign = "right";
			}
			else
				authorBubble.textContent = message.Sender;
			
			this.messageContainer.appendChild(authorBubble);
		}

		// Message box
		var messageBubble = document.createElement("div");
		messageBubble.setAttribute('class', 'message');
		messageBubble.textContent = decryptedText;
		this.messageContainer.appendChild(messageBubble);

		if (senderIsCurrentUser) {
			messageBubble.style.textAlign = "right";
            messageBubble.style.backgroundColor = "#3000a8";
        }
		if (sameSenderAsLastMessage) {
			messageBubble.style.marginTop = "-13px";
		}

		this.lastMessageAuthor = message.Sender;
		this.UpdateScroll();
	}

	Notification(message) {
		if (message === null || message === "") return;
		var notification = document.createElement("div");
		notification.setAttribute('class', 'notification');
		notification.textContent = message;
		this.messageContainer.appendChild(notification);
		this.lastMessageAuthor = "system";
		this.UpdateScroll();
	}

	// Helpers

	UpdateScroll() {
		this.messageContainer.scrollTop = this.messageContainer.scrollHeight;
	}
}
