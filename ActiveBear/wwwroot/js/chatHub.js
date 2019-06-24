"use strict";

class ChatHub {
	const SendMessage = "SendMessage";
	const GetChannelMessages = "GetChannelMessages";

	static PostMessage() {
		if (message === null || message === "") return;
		if (passwordInput.value === "" || passwordInput.value === null)
			return PasswordEmptyError();

		var message = AesEncrypt(messageInput.value);
		var messagePacket = GenerateMessagePacket(message);
		connection.invoke(SendMessage, messagePacket).catch(function (err) {
			return console.error(err.toString());
		});
		messageInput.value = "";
	}

	static RequestAllMessages() {
		connection.invoke(GetChannelMessages, GenerateChannelPacket()).catch(function (err) {
			return console.error(err.toString());
		});
	}

	static RemoveAllMessages() {
		while (messageContainer.firstChild) {
			messageContainer.removeChild(messageContainer.firstChild);
		}
	}
}
