# ActiveBear
An Encrypted Messaging webapp written in C# / ASP.NET core.

## Security Features

The purpose of this repo is to provide a simple, lightweight and easy to implement encrypted messaging solution.

ActiveBear holds a number of user-created 'channels', each with a unique key.
To be able to access a channel, users must first auth to a channel by entering the correct channel key. After that, they will have to enter the password on every page load.

Keys are hashed to a SHA-256 byte array ([see here](https://github.com/emn178/js-sha256)), then used to encrypt messages with an [AES-OFB block cipher](https://github.com/ricmoo/aes-js) algorithm.

Messages are encrypted end-to-end, so they are completely secure during transit and when stored. Decrypting is done on the fly and can only happen when the correct key has been supplied.

## Privacy features

ActiveBear uses one identity cookie to associate a web user with an account. This cookie stores no information itself except a reference to the account ID.

No logs are kept. Note however the ISP/provider of the server may.

Client does not use analytics or any willful third-party tracking.

## How to run it

Clone the repo and open it with VisualStudio or your preferred alternate.
In the `ActiveBear/ActiveBear/` directory, npm install the two encryption libraries:
  * [js-sha256](https://github.com/emn178/js-sha256)
  * [aes-js](https://github.com/ricmoo/aes-js)
  
And SignalR, the microsoft package used for server-client communication:

  * [signalr](https://www.npmjs.com/package/@aspnet/signalr)
  
Next, build the project in your IDE. When running, you should be able to use it on:

`https://localhost:5001`

## How can I help to develop it?
