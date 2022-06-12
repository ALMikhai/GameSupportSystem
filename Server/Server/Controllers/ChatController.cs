using DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Services.Redis;

namespace Server.Controllers
{
	/// <summary>
	/// Controller that determines the functionality of working with chat.
	/// </summary>
    public class ChatController : Controller
    {
		private readonly Models.AppContext context;
		private readonly IRedisCacheService redisCache;

		/// <summary>
		/// Controller that determines the functionality of working with chat.
		/// </summary>
		/// <param name="context">Application database context.</param>
		/// <param name="redisCache">Redis layer for caching.</param>
		public ChatController(Models.AppContext context, IRedisCacheService redisCache) {
			this.context = context;
			this.redisCache = redisCache;
		}

		/// <summary>
		/// Main page with list of chats.
		/// </summary>
		/// <returns>Index page.</returns>
		[Authorize(Roles = "Operator, Observer")]
		public IActionResult Index() {
			var uniqueChats = context.Messages.AsEnumerable().DistinctBy(m => m.ChatId).ToList();
			return View(uniqueChats.Select(c => new Tuple<Message, int>(c, NumOfUnreadMessages(c.ChatId, Message.SourceType.Player))));
		}

		/// <summary>
		/// Player chat page.
		/// </summary>
		/// <param name="chatId">Id of chat.</param>
		/// <returns>Chat page.</returns>
		[Authorize(Roles = "Operator, Observer")]
		public IActionResult Chat(Guid chatId) {
			var messages = context.Messages.Where(m => m.ChatId == chatId);
			var chatModel = new Tuple<Guid, IEnumerable<Message>>(chatId, messages);
			if (User.IsInRole("Observer")) {
				return View("OnlyViewChat", chatModel);
			} else {
				return base.View(chatModel);
			}
		}

		/// <summary>
		/// Request for get past messages.
		/// </summary>
		/// <param name="chatId">Id of chat.</param>
		/// <returns>Json of Message[].</returns>
		public IActionResult ChatHistory(Guid chatId) {
			return Json(context.Messages.AsEnumerable().Where(m => m.ChatId == chatId).ToArray());
		}

		/// <summary>
		/// Request to get the number of unread messages from the operator.
		/// </summary>
		/// <param name="chatId">Id of chat.</param>
		/// <returns>Number of unread messages from the operator.</returns>
		public ActionResult<int> NumOfUnreadOperatorMessages(Guid chatId) {
			int result;
			if (redisCache.Get<bool>($"{chatId}-numUnreadMessagesCached")) {
				result = redisCache.Get<int>($"{chatId}-numUnreadMessages");
			} else {
				var numUnreadMessage = NumOfUnreadMessages(chatId, Message.SourceType.Operator);
				redisCache.Set<int>($"{chatId}-numUnreadMessages", numUnreadMessage);
				redisCache.Set<bool>($"{chatId}-numUnreadMessagesCached", true);
				result = numUnreadMessage;
			}
			return result;
		}

		private int NumOfUnreadMessages(Guid chatId, Message.SourceType sourceType) {
			var messages = context.Messages.Where(m => m.ChatId == chatId);
			return messages.Count(m => m.Type == sourceType && m.IsReaded == false);
		}
	}
}
