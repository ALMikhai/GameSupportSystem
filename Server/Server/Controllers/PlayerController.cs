using DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Controllers
{
	/// <summary>
	/// Controller that determines the functionality of working with players.
	/// </summary>
	public class PlayerController : Controller
	{
		private readonly Models.AppContext context;

		/// <summary>
		/// Controller that determines the functionality of working with players.
		/// </summary>
		/// <param name="context">Application database context.</param>
		public PlayerController(Models.AppContext context) {
			this.context = context;
		}

		/// <summary>
		/// Registeration request with fill model.
		/// </summary>
		/// <param name="deviceId">Unique device id.</param>
		/// <param name="nickname">Player nickname.</param>
		/// <returns>If the registration was successful, return success RegistrationResponse with token,
		/// else return error RegistrationResponse with error message.</returns>
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

		/// <summary>
		/// Login request with fill model.
		/// </summary>
		/// <param name="deviceId">Unique device id.</param>
		/// <param name="nickname">Player nickname.</param>
		/// <returns>If the login was successful, return success LoginResponse with token,
		/// else return error LoginResponse with error message.</returns>
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
