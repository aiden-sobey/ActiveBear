"use strict";

class Encryption {
	static AesEncrypt(text) {
		var key = HashChannelPassword();
		var iv = [ 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36 ];

		// Convert text to bytes
		var textBytes = aesjs.utils.utf8.toBytes(text);

		var aesOfb = new aesjs.ModeOfOperation.ofb(key, iv);
		var encryptedBytes = aesOfb.encrypt(textBytes);

		return aesjs.utils.hex.fromBytes(encryptedBytes);
	}

	static AesDecrypt(encryptedHex) {
		var key = HashChannelPassword();
		var iv = [ 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36 ];

		var encryptedBytes = aesjs.utils.hex.toBytes(encryptedHex);
		var aesOfb = new aesjs.ModeOfOperation.ofb(key, iv);

		try {
			var decryptedBytes = aesOfb.decrypt(encryptedBytes);
			return aesjs.utils.utf8.fromBytes(decryptedBytes);
		}
		catch (err) {
			return encryptedHex;
		}
	}

	static HashChannelPassword() {
		// Get a 256-bit key hash
		var hashedPassword = sha256(passwordInput.value);
		return aesjs.utils.hex.toBytes(hashedPassword);
	}
}
