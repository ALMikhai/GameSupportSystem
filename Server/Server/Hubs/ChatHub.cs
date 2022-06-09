using DataModels;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Server.Hubs
{
	public class ChatHub : Hub
	{
		private readonly Models.AppContext context;
		private readonly ILogger<ChatHub> logger;

		public ChatHub(Models.AppContext context, ILogger<ChatHub> logger) {
			this.context = context;
			this.logger = logger;
		}

		public async Task SendToChatFromPlayer(Guid playerTokenId, string message) {
			if (!context.Tokens.Any(t => t.Id == playerTokenId)) {
				await SendError(playerTokenId, "This token is not valid, please re-login.");
				return;
			}
			var token = await context.Tokens.FirstAsync(t => t.Id == playerTokenId);
			var player = await context.Players.FirstAsync(p => p.DeviceId == token.PlayerDeviceId);
			var nickname = player.Nickname;
			context.Messages.Add(new Message() {
				ChatId = playerTokenId,
				Name = nickname,
				Text = message,
				Type = Message.SourceType.Player,
			});
			await context.SaveChangesAsync();
			await ReceiveMessage(playerTokenId, nickname, message);
		}

		public async Task SendToChatFromOperator(Guid playerTokenId, string message) {
			if (!context.Tokens.Any(t => t.Id == playerTokenId)) {
				logger.LogError($"Token is not valid {playerTokenId}");
				return;
			}
			var name = "Operator";
			context.Messages.Add(new Message() {
				ChatId = playerTokenId,
				Name = name,
				Text = message,
				Type = Message.SourceType.Operator,
			});
			await context.SaveChangesAsync();
			await ReceiveMessage(playerTokenId, name, message);
		}

		private async Task ReceiveMessage(Guid playerTokenId, string sourceName, string message) {
			await Clients.All.SendAsync($"ReceiveMessage-{playerTokenId}", sourceName, message);
		}

		private async Task SendError(Guid playerTokenId, string message) {
			await Clients.All.SendAsync($"ReceiveError-{playerTokenId}", message);
		}
	}
}
