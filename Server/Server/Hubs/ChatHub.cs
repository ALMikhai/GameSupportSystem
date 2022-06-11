using DataModels;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Server.Services.Redis;
using System.Text.Json;

namespace Server.Hubs
{
	/// <summary>
	/// Hub containing functionality for messaging between the player and the operator.
	/// </summary>
	public class ChatHub : Hub
	{
		private readonly Models.AppContext context;
		private readonly ILogger<ChatHub> logger;
		private readonly IRedisCacheService redisCache;

		/// <summary>
		/// Hub containing functionality for messaging between the player and the operator.
		/// </summary>
		/// <param name="context">Application database context.</param>
		/// <param name="logger">Logger object.</param>
		/// <param name="redisCache">Redis layer for caching.</param>
		public ChatHub(Models.AppContext context, ILogger<ChatHub> logger, IRedisCacheService redisCache) {
			this.context = context;
			this.logger = logger;
			this.redisCache = redisCache;
		}

		/// <summary>
		/// Request to send messages from the player.
		/// Saves the message to the database and sends this message to all users associated with this chat.
		/// </summary>
		/// <param name="playerTokenId">Player token.</param>
		/// <param name="message">Text of message.</param>
		public async Task SendToChatFromPlayer(Guid playerTokenId, string message) {
			if (!context.Tokens.Any(t => t.Id == playerTokenId)) {
				await SendError(playerTokenId, "This token is not valid, please re-login.");
				return;
			}
			await MarkOperatorMessagesAsRead(playerTokenId);
			var token = await context.Tokens.FirstAsync(t => t.Id == playerTokenId);
			var player = await context.Players.FirstAsync(p => p.DeviceId == token.PlayerDeviceId);
			var nickname = player.Nickname;
			var messageObject = new Message() {
				ChatId = playerTokenId,
				Name = nickname,
				Text = message,
				Type = Message.SourceType.Player,
			};
			context.Messages.Add(messageObject);
			await context.SaveChangesAsync();
			await ReceiveMessage(playerTokenId, messageObject);
		}

		/// <summary>
		/// Request to send messages from the operator.
		/// Saves the message to the database and sends this message to all users associated with this chat.
		/// </summary>
		/// <param name="playerTokenId">Player token.</param>
		/// <param name="message">Text of message.</param>
		public async Task SendToChatFromOperator(Guid playerTokenId, string message) {
			if (!context.Tokens.Any(t => t.Id == playerTokenId)) {
				logger.LogError($"Token is not valid {playerTokenId}");
				return;
			}
			await MarkPlayerMessagesAsRead(playerTokenId);
			var name = "Operator";
			var messageObject = new Message() {
				ChatId = playerTokenId,
				Name = name,
				Text = message,
				Type = Message.SourceType.Operator,
			};
			context.Messages.Add(messageObject);
			await context.SaveChangesAsync();
			redisCache.Set<bool>($"{playerTokenId}-numUnreadMessagesCached", false);
			await ReceiveMessage(playerTokenId, messageObject);
		}

		/// <summary>
		/// Marks all messages from the player as read.
		/// </summary>
		/// <param name="chatId">Chat id.</param>
		public async Task MarkPlayerMessagesAsRead(Guid chatId) {
			await MarkMessagesAsRead(chatId, Message.SourceType.Player);
		}

		/// <summary>
		/// Marks all messages from the operator as read.
		/// </summary>
		/// <param name="chatId">Chat id.</param>
		public async Task MarkOperatorMessagesAsRead(Guid chatId) {
			redisCache.Set<bool>($"{chatId}-numUnreadMessagesCached", false);
			await MarkMessagesAsRead(chatId, Message.SourceType.Operator);
		}

		private async Task ReceiveMessage(Guid playerTokenId, Message message) {
			await Clients.All.SendAsync($"ReceiveMessage-{playerTokenId}", JsonConvert.SerializeObject(message));
		}

		private async Task SendError(Guid playerTokenId, string message) {
			await Clients.All.SendAsync($"ReceiveError-{playerTokenId}", message);
		}

		private async Task MarkMessagesAsRead(Guid chatId, Message.SourceType from) {
			var messages = context.Messages.Where(m => m.ChatId == chatId);
			foreach (var message in messages) {
				if (message.Type == from) {
					message.IsReaded = true;
				}
			}
			await context.SaveChangesAsync();
		}
	}
}
