using DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Controllers
{
	public class PlayerController : Controller
	{
		private readonly Models.AppContext context;

		public PlayerController(Models.AppContext context) {
			this.context = context;
		}

		public async Task<JsonResult> Register(string deviceId, string nickname) {
			if (string.IsNullOrEmpty(deviceId) || string.IsNullOrEmpty(nickname)) {
				return Json(new RegistrationResponse() {
					Type = RegistrationResponse.ResponseType.Error,
					ErrorMessage = "Invalid nickname or/and device id.",
				});
			}
			if (nickname.Length > 16) {
				return Json(new RegistrationResponse() {
					Type = RegistrationResponse.ResponseType.Error,
					ErrorMessage = "Nickname must be less than 16 characters.",
				});
			}
			if (context.Players.Any(p => p.DeviceId == deviceId && p.Nickname == nickname)) {
				return Json(new RegistrationResponse() {
					Type = RegistrationResponse.ResponseType.Error,
					ErrorMessage = "This player is already registered.",
				});
			}
			if (context.Players.Any(p => p.DeviceId == deviceId)) {
				return Json(new RegistrationResponse() {
					Type = RegistrationResponse.ResponseType.Error,
					ErrorMessage = "This device is already registered.",
				});
			}
			var player = new Player() { DeviceId = deviceId, Nickname = nickname, };
			context.Players.Add(player);
			var token = new PlayerToken() { Id = Guid.NewGuid(), PlayerDeviceId = deviceId, };
			context.Tokens.Add(token);
			context.SaveChanges();
			return Json(new RegistrationResponse() {
				Type = RegistrationResponse.ResponseType.Succes,
				AccessToken = token.Id,
			});
		}

		public async Task<JsonResult> Login(string deviceId, string nickname) {
			if (string.IsNullOrEmpty(deviceId) || string.IsNullOrEmpty(nickname)) {
				return Json(new LoginResponse() {
					Type = LoginResponse.ResponseType.Error,
					ErrorMessage = "Invalid nickname or/and device id.",
				});
			}
			if (!context.Players.Any(p => p.DeviceId == deviceId && p.Nickname == nickname)) {
				return Json(new LoginResponse() {
					Type = LoginResponse.ResponseType.Error,
					ErrorMessage = "This player is not registered.",
				});
			}
			var playerTokenId = await context.Tokens.FirstAsync(p => p.PlayerDeviceId == deviceId);
			return Json(new LoginResponse() {
				Type = LoginResponse.ResponseType.Succes,
				AccessToken = playerTokenId.Id,
			});
		}
	}
}
