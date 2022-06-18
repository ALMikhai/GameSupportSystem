# GameSupportSystem

The system consists of the following parts:
- administrative interface - here operators read and respond to messages from players;
- api for processing requests from the game;
- client library that uses api to communicate with the server from the game.

## Administrative interface
Authorization - Operators can log in with email and password.

List of dialogues - the operator can see the list of dialogues from the players.

Dialogue page - the operator can see the dialogue with the player.

Response form on the dialog page - the operator can write a new message.

## Http api
- Player registration - adds a new player to the system;
- Number of unread messages.

## SignalR
- Player authorization by Device Token;
- Get a list of posts with pagination and the ability to request the next “page”;
- Send a message;
- Mark the message as read.

## Roles
- Guest - can only log in;
- Observer - can only read;
- Operator - can both read and write.

## Redis
Add server side caching to query the number of unread messages.

## Client on Unity
The client implements Http api and SignalR api.
To run the client, you will need to install the Best HTTP library and TMPro.
